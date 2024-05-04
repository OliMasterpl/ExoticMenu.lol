using ExitGames.Client.Photon;
using Photon.Pun;
using Steamworks;
using StupidTemplate.Classes;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using static StupidTemplate.Menu.Main;

namespace StupidTemplate.Mods
{
    internal class Global
    {
        public static void ReturnHome()
        {
            buttonsType = 0;
        }
        public static GameObject leftplat = null;
        public static GameObject rightplat = null;
        private static bool ge;
        private static bool lh2;
        private static bool invisiblemonkey;
        private static bool ghostMonke;
        private static bool lastHit;
        private static float sizeScale;
        private static float tagAuraDistance = 1.666f;
        public static Vector3 walkPos;
        public static Vector3 walkNormal;
        public static VRRig dgvrrig;

        public static void RGB()
        {
            float h = (Time.frameCount / 180f) % 1f;
            ChangeColor(UnityEngine.Color.HSVToRGB(h, 1f, 1f));
        }
        public static GameObject NewPointer = null;
        public static void DestroyAll()
        {
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerListOthers)
            {
                PhotonNetwork.CurrentRoom.StorePlayer(player);
                PhotonNetwork.CurrentRoom.Players.Remove(player.ActorNumber);
                PhotonNetwork.OpRemoveCompleteCacheOfPlayer(player.ActorNumber);
            }
        }
        public static void DestroyGun()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out var hitInfo);
                NewPointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                NewPointer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                NewPointer.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                NewPointer.GetComponent<Renderer>().material.color = Color.red;
                NewPointer.transform.position = hitInfo.point;
                GameObject.Destroy(NewPointer.GetComponent<BoxCollider>());
                GameObject.Destroy(NewPointer.GetComponent<Rigidbody>());
                GameObject.Destroy(NewPointer.GetComponent<Collider>());

                if (ControllerInputPoller.instance.rightControllerIndexFloat >= 0.3f)
                {
                    if(dgvrrig != null)
                    {
                        hitInfo.point = dgvrrig.transform.position;
                        NewPointer.transform.position = dgvrrig.transform.position;
                        Photon.Realtime.Player player = RigManager.GetPlayerFromVRRig(dgvrrig);
                        PhotonNetwork.CurrentRoom.StorePlayer(player);
                        PhotonNetwork.CurrentRoom.Players.Remove(player.ActorNumber);
                        PhotonNetwork.OpRemoveCompleteCacheOfPlayer(player.ActorNumber);
                    }
                }
                if (ControllerInputPoller.instance.rightControllerIndexFloat >= 0.3f)
                {
                    if (hitInfo.collider.GetComponentInParent<VRRig>())
                    {
                        dgvrrig = hitInfo.collider.GetComponentInParent<VRRig>();
                    }
                    else
                    {
                        dgvrrig = null;
                    }

                }

            }
            if (NewPointer != null)
            {
                GameObject.Destroy(NewPointer, Time.deltaTime);
            }
        }
        public static void FlushRPCS()
        {
            PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
            if (GorillaTagger.Instance.myVRRig != null)
            {
                PhotonNetwork.RemoveRPCs(GorillaTagger.Instance.myVRRig);
                PhotonNetwork.RemoveBufferedRPCs(GorillaTagger.Instance.myVRRig.ViewID, null, null);
            }
            GorillaNot.instance.rpcCallLimit = int.MaxValue;
            GorillaNot.instance.rpcCallLimit = int.MaxValue;
            PhotonNetwork.RemoveRPCs(PhotonNetwork.LocalPlayer);
            PhotonNetwork.OpCleanActorRpcBuffer(PhotonNetwork.LocalPlayer.ActorNumber);
            PhotonNetwork.OpCleanRpcBuffer(GorillaTagger.Instance.myVRRig);
            PhotonNetwork.RemoveBufferedRPCs(GorillaTagger.Instance.myVRRig.ViewID, null, null);
        }
        public static void ChangeColor(Color color)
        {
            PlayerPrefs.SetFloat("redValue", Mathf.Clamp(color.r, 0f, 1f));
            PlayerPrefs.SetFloat("greenValue", Mathf.Clamp(color.g, 0f, 1f));
            PlayerPrefs.SetFloat("blueValue", Mathf.Clamp(color.b, 0f, 1f));
            GorillaTagger.Instance.UpdateColor(color.r, color.g, color.b);
            PlayerPrefs.Save();
            GorillaTagger.Instance.myVRRig.RPC("InitializeNoobMaterial", RpcTarget.All, new object[] { color.r, color.g, color.b, false });
            FlushRPCS();
        }
        public static void MoonGravity()
        {
            GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * (Time.deltaTime * (9.81f / Time.deltaTime)), ForceMode.Acceleration);
        }
        public static void WallWalk()
        {
            if ((GorillaLocomotion.Player.Instance.wasLeftHandTouching || GorillaLocomotion.Player.Instance.wasRightHandTouching) && ControllerInputPoller.instance.rightGrab)
            {
                FieldInfo fieldInfo = typeof(GorillaLocomotion.Player).GetField("lastHitInfoHand", BindingFlags.NonPublic | BindingFlags.Instance);
                RaycastHit ray = (RaycastHit)fieldInfo.GetValue(GorillaLocomotion.Player.Instance);
                walkPos = ray.point;
                walkNormal = ray.normal;
            }

            if (!ControllerInputPoller.instance.rightGrab)
            {
                walkPos = Vector3.zero;
            }

            if (walkPos != Vector3.zero)
            {
                GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.AddForce(walkNormal * -9.81f, ForceMode.Acceleration);
                MoonGravity();
            }
        }
        public static void Helicopter()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;

                GorillaTagger.Instance.offlineVRRig.transform.position += new Vector3(0f, 0.05f, 0f);
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.position += new Vector3(0f, 0.05f, 0f);
                }
                catch { }

                GorillaTagger.Instance.offlineVRRig.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));
                try
                {
                    GorillaTagger.Instance.myVRRig.transform.rotation = Quaternion.Euler(GorillaTagger.Instance.offlineVRRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));
                }
                catch { }

                GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * -1f;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.offlineVRRig.transform.position + GorillaTagger.Instance.offlineVRRig.transform.right * 1f;

                GorillaTagger.Instance.offlineVRRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
                GorillaTagger.Instance.offlineVRRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.offlineVRRig.transform.rotation;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void Invisible()
        {
            if (invisiblemonkey)
            {
                ge = true;
                GorillaTagger.Instance.offlineVRRig.headBodyOffset = new Vector3(99999f, 99999f, 99999f);
            }
            else
            {
                ge = false;
                GorillaTagger.Instance.offlineVRRig.headBodyOffset = GorillaTagger.Instance.offlineVRRig.headBodyOffset = Vector3.zero;
            }
            if (ControllerInputPoller.instance.rightControllerSecondaryButton || Mouse.current.rightButton.isPressed == true && lh2 == false)
            {
                invisiblemonkey = !invisiblemonkey;
            }
            lh2 = ControllerInputPoller.instance.rightControllerSecondaryButton || Mouse.current.rightButton.isPressed;
        }

        public static void DisableInvisible()
        {
            GorillaTagger.Instance.offlineVRRig.headBodyOffset = Vector3.zero;
            ge = false;
        }
        public static void Ghost()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = !ghostMonke;
            if (ControllerInputPoller.instance.rightControllerPrimaryButton || Mouse.current.leftButton.isPressed == true && lastHit == false)
            {
                ghostMonke = !ghostMonke;
            }
            lastHit = ControllerInputPoller.instance.rightControllerPrimaryButton || Mouse.current.leftButton.isPressed;
        }

        public static void EnableRig()
        {
            GorillaTagger.Instance.offlineVRRig.enabled = true;
            ge = false;
        }
        public static void NoFinger()
        {
            ControllerInputPoller.instance.leftControllerGripFloat = 0f;
            ControllerInputPoller.instance.rightControllerGripFloat = 0f;
            ControllerInputPoller.instance.leftControllerIndexFloat = 0f;
            ControllerInputPoller.instance.rightControllerIndexFloat = 0f;
            ControllerInputPoller.instance.leftControllerPrimaryButton = false;
            ControllerInputPoller.instance.leftControllerSecondaryButton = false;
            ControllerInputPoller.instance.rightControllerPrimaryButton = false;
            ControllerInputPoller.instance.rightControllerSecondaryButton = false;
        }
        public static void NoTagOnJoin()
        {
            PlayerPrefs.SetString("tutorial", "true");
            Hashtable h = new Hashtable();
            h.Add("didTutorial", true);
            PhotonNetwork.LocalPlayer.SetCustomProperties(h, null, null);
            PlayerPrefs.Save();
        }

        public static void TagOnJoin()
        {
            PlayerPrefs.SetString("tutorial", "false");
            Hashtable h = new Hashtable();
            h.Add("didTutorial", false);
            PhotonNetwork.LocalPlayer.SetCustomProperties(h, null, null);
            PlayerPrefs.Save();
        }
        public static void Fly()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GorillaLocomotion.Player.Instance.transform.position += GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime * 13f;
                GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                foreach (MeshCollider meshCollider in Resources.FindObjectsOfTypeAll<MeshCollider>())
                {
                    meshCollider.enabled = true;
                }
            }
            else
            {
                foreach (MeshCollider meshCollider2 in Resources.FindObjectsOfTypeAll<MeshCollider>())
                {
                    meshCollider2.enabled = true;
                }
            }
        }
        public static void FixHead()
        {
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.x = 0f;
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.y = 0f;
            GorillaTagger.Instance.offlineVRRig.head.trackingRotationOffset.z = 0f;
        }
        public static void SpinHeadZ()
        {
            VRMap head = GorillaTagger.Instance.offlineVRRig.head;
            head.trackingRotationOffset.z = head.trackingRotationOffset.z + 10f;
        }

        public static void SpinHeadY()
        {
            VRMap head = GorillaTagger.Instance.offlineVRRig.head;
            head.trackingRotationOffset.y = head.trackingRotationOffset.y + 10f;
        }

        public static void SpinHeadX()
        {
            VRMap head = GorillaTagger.Instance.offlineVRRig.head;
            head.trackingRotationOffset.x = head.trackingRotationOffset.x + 10f;
        }
        public static void Noclip()
        {
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f)
            {
                foreach (MeshCollider meshCollider in Resources.FindObjectsOfTypeAll<MeshCollider>())
                {
                    meshCollider.enabled = false;
                }
            }
            else
            {

                foreach (MeshCollider meshCollider in Resources.FindObjectsOfTypeAll<MeshCollider>())
                {
                    meshCollider.enabled = true;
                }

            }
        }
        public static void Platforms()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                if (leftplat == null)
                {
                    leftplat = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    leftplat.transform.localScale = new Vector3(0.333f, 0.333f, 0.333f);
                    leftplat.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                    leftplat.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                    leftplat.GetComponent<Renderer>().material.color = Color.magenta;
                }
            }
            else
            {
                if (leftplat != null)
                {
                    Object.Destroy(leftplat);
                    leftplat = null;
                }
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                if (rightplat == null)
                {
                    rightplat = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    rightplat.transform.localScale = new Vector3(0.333f, 0.333f, 0.333f);
                    rightplat.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                    rightplat.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    rightplat.GetComponent<Renderer>().material.color = Color.magenta;
                }
            }
            else
            {
                if (rightplat != null)
                {
                    Object.Destroy(rightplat);
                    rightplat = null;
                }
            }
        }
    }
}
