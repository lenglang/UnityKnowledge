using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System;
    public enum UVAnimaType
    {
        In,
        Out
    }
    /// <summary>
    /// ControlUV.Instance.StartAnima(UVAnimaType.In) 此函数有3个重载函数，选用一个
    /// </summary>
    public class ControlUV : Singleton<ControlUV>
    {
        private static ControlUV instance;

        public static ControlUV Ins
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType(typeof(ControlUV)) as ControlUV;
                if (instance == null)
                {
                    Debug.Log("ControlUV实例为空");
                }
                return instance;
            }
        }
        public float AnimaTime = 1;
        public UVAnimaType type = UVAnimaType.In;
        [Tooltip("自动播放")]
        public bool isPlayAwake = false;

        public Action OnComplete;//回调
        bool isPlaying = false;
        float Size = 1;
        public GameObject overlay;
        Material material;//请确保material正确获取

        public Transform whiteTransfrom;
        public float whiteAnimaTime = 1f;
        protected override void Awake()
        {

            material = overlay.GetComponent<MeshRenderer>().material;

            //else
            overlay.SetActive(false);
            if (isPlayAwake)
                StartWhiteAnima(type);
                //LeadQiQiAnima(delegate {},delegate {});
                //StartQiQiAnima(UVAnimaType.In,delegate {StartQiQiAnima(UVAnimaType.Out);});
                //StartQiQiAnima(type, 0, 0);

        }

        public void LeadQiQiAnima(Action from, Action to)
        {
            if (from == null || to == null)
            {
                Debug.LogError("请传事件");
            }
            StartQiQiAnima(UVAnimaType.In, () =>
            {
                from();
                StartQiQiAnima(UVAnimaType.Out, () => { to(); });
            });
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="type"></param>
        public void StartQiQiAnima(UVAnimaType type, float size1, float size2, Action complete = null)
        {
            if (isPlaying)
                return;
            OnComplete = complete;
            StartCoroutine(StartUV(type, size1, size2));
            OnComplete = null;
        }
        /// <summary>
        /// 回调事件/
        /// </summary>
        public void StartQiQiAnima(UVAnimaType type, Action complete)
        {
            //if (isPlaying)
            //    return;
            OnComplete = complete;
            StartCoroutine(StartUV(type));
        }
        IEnumerator StartUV(UVAnimaType type)
        {
            if (type == UVAnimaType.In)
            {
                //SoundManager.Main.PlayFromPrefab(SoundType.音效, "010007过场收起");
            }
            else if (type == UVAnimaType.Out)
            {
                //SoundManager.Main.PlayFromPrefab(SoundType.音效, "010008过场展开");
            }
            isPlaying = true;
            overlay.gameObject.SetActive(true);
            if (material == null)
                material = overlay.GetComponent<MeshRenderer>().material;

            float startSize = 0;
            float targetSize = 0;
            if (type == UVAnimaType.In)
            {
                startSize = 0.2f;
                targetSize = 50f;
            }
            else
            {
                startSize = 50f;
                targetSize = 0.2f;
            }
            float time = 0;
            float outScale = 10;
            while (true)
            {
                if (type == UVAnimaType.In)
                {
                    time += Time.deltaTime / AnimaTime;
                }
                else
                {
                    outScale = Mathf.Lerp(outScale, 1, time);
                    time += Time.deltaTime / AnimaTime * outScale;
                }

                Size = Mathf.Lerp(startSize, targetSize, time);

                Vector2 tiling = new Vector2(Size, Size);
                float offsetScale = -(Size - 1) * 0.5f;
                Vector2 offset = new Vector2(offsetScale, offsetScale);

                material.SetTextureScale("_MainTex", tiling);
                material.SetTextureOffset("_MainTex", offset);
                if (time > AnimaTime)
                {
                    if (OnComplete != null)
                        OnComplete();
                    isPlaying = false;
                    break;
                }
                yield return null;
            }
            if (type == UVAnimaType.Out)
                overlay.gameObject.SetActive(false);

            //yield return new WaitForSeconds(1f);
            //isPlaying = false;
        }

        IEnumerator StartUV(UVAnimaType type, float size1, float size2)
        {
            isPlaying = true;
            overlay.gameObject.SetActive(true);
            if (material == null)
                material = overlay.GetComponent<MeshRenderer>().material;

            float startSize = 0;
            float targetSize = 0;
            //if (type == UVAnimaType.In)
            //{
            //    startSize = 0.2f;
            //    targetSize = 50f;
            //}
            //else
            //{
            //    startSize = 50f;
            //    targetSize = 0.2f;
            //}
            if (type == UVAnimaType.In)
            {
                startSize = size1;
                targetSize = size2;
            }
            else
            {
                startSize = size2;
                targetSize = size1;
            }
            float time = 0;
            float outScale = 10;
            while (true)
            {
                if (type == UVAnimaType.In)
                {
                    time += Time.deltaTime / AnimaTime;
                }
                else
                {
                    outScale = Mathf.Lerp(outScale, 1, time);
                    time += Time.deltaTime / AnimaTime * outScale;
                }

                Size = Mathf.Lerp(startSize, targetSize, time);


                Vector2 tiling = new Vector2(Size, Size);

                //float offsetScale = -(Size - 1) * 0.5f;

                float offsetScaleX = -0.32f * Size + 0.32f;
                float offsetScaleY = -0.6f * Size + 0.6f;

                Vector2 offset = new Vector2(offsetScaleX, offsetScaleY);


                material.SetTextureScale("_MainTex", tiling);
                material.SetTextureOffset("_MainTex", offset);

                if (time > AnimaTime)
                {
                    isPlaying = false;
                    if (OnComplete != null)
                        OnComplete();
                    break;
                }
                yield return null;
            }
            if (type == UVAnimaType.Out)
                overlay.gameObject.SetActive(false);

            //yield return new WaitForSeconds(1f);
            //isPlaying = false;
        }
        /// <summary>
        /// In:变白色
        /// out:变透明
        /// </summary>
        /// <param name="type"></param>
        public void StartWhiteAnima(UVAnimaType type, Action onComplete = null)
        {
            Material mate = whiteTransfrom.GetComponent<MeshRenderer>().material;
            if (type == UVAnimaType.In)
            {
                mate.color = new Color(1, 1, 1, 0);
                if (onComplete == null)
                    mate.DOColor(Color.white, whiteAnimaTime);
                else
                    mate.DOColor(Color.white, whiteAnimaTime).OnComplete(() => { onComplete(); });
            }
            else
            {
                mate.color = Color.white;
                if (onComplete == null)
                    mate.DOColor(new Color(1, 1, 1, 0), whiteAnimaTime);
                else
                    mate.DOColor(new Color(1, 1, 1, 0), whiteAnimaTime).OnComplete(() => { onComplete(); });
            }
        }
        /// <summary>
        /// first,屏幕白色的时候做的事,two,完成切场
        /// </summary>
        /// <param name="first"></param>
        /// <param name="two"></param>
        public void LoadWhiteAnima(Action first, Action two)
        {
            if (first == null || two == null)
            {
                Debug.LogError("请传事件");
            }
            StartWhiteAnima(UVAnimaType.In, () => { first(); Next(two); });
        }
        void Next(Action two)
        {
            StartWhiteAnima(UVAnimaType.Out, () => { two(); });
        }
        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Alpha1))
        //        StartWhiteAnima(UVAnimaType.In);
        //    if (Input.GetKeyDown(KeyCode.Alpha2))
        //        StartWhiteAnima(UVAnimaType.Out);
        //}
        protected override void OnDestroy()
        {
            instance = null;
        }
    }

