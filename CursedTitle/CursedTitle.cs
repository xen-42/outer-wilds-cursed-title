using OWML.ModHelper;
using OWML.Common;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

namespace CursedTitle
{
    public class CursedTitle : ModBehaviour
    {
        private TitleAnimationController gfxController;

        private void Start()
        {
            ModHelper.Console.WriteLine($"CursedTitle loaded", MessageType.Info);
            SceneManager.sceneLoaded += OnSceneLoaded;

            //TitleScreen is already open
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            if (gfxController != null) gfxController.OnTitleLogoAnimationComplete -= OnTitleLogoAnimationComplete;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "TitleScreen") return;

            ModHelper.Console.WriteLine("TitleScreen loaded", MessageType.Info);

            GameObject.Find("TitleCanvasHack/TitleLayoutGroup/OW_Logo_Anim/OW_Logo_Anim/OUTER").transform.localScale = Vector3.zero;
            GameObject.Find("TitleCanvasHack/TitleLayoutGroup/OW_Logo_Anim/OW_Logo_Anim/WILDS").transform.localScale = Vector3.zero;

            gfxController = GameObject.Find("TitleMenuManagers").GetComponent<TitleScreenManager>()._gfxController;
            gfxController.OnTitleLogoAnimationComplete += OnTitleLogoAnimationComplete;
        }

        public void OnTitleLogoAnimationComplete()
        {
            Texture2D newLogo;
            UnityEngine.Random.InitState(Guid.NewGuid().GetHashCode());
            if(UnityEngine.Random.Range(0, 100) < 99)
            {
                newLogo = ModHelper.Assets.GetTexture("real_logo_alt.png");
            }
            else
            {
                newLogo = ModHelper.Assets.GetTexture("real_logo.png");
            }

            var logoSize = new Vector2(newLogo.width, newLogo.height);

            var logo = GameObject.Find("TitleCanvasHack/TitleLayoutGroup/OW_Logo_Anim/OW_Logo_Anim");
            var image = logo.AddComponent<UnityEngine.UI.Image>();
            image.sprite = Sprite.Create(newLogo, new Rect(Vector2.zero, logoSize), logoSize / 2f);

            var root = GameObject.Find("TitleCanvasHack/TitleLayoutGroup/OW_Logo_Anim");
            root.transform.localRotation = Quaternion.Euler(0, 0, 0);
            root.transform.localScale = new Vector3(5, 2.5f, 1);
        }
    }
}
