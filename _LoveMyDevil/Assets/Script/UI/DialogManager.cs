using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DialogManager : MonoBehaviour
{
    [SerializeField] private Text _mainText;

    private bool isSkip=false;
    // Start is called before the first frame update
    void Start()
    {
        TypingText("안녕 반가워 나는 텍스트야.").Forget();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSkip && Input.GetKeyDown(KeyCode.Return))
        {
            TypingText("테스트 메세지를 출력하는 중 이지.").Forget();
        }
    }

    async UniTaskVoid TypingText(string pText,float dur = 0.75f)
    {
        isSkip = false;
        _mainText.text = null;
        _mainText.DOText(pText, 0.75f);
        await UniTask.Delay(TimeSpan.FromSeconds(dur+0.1f));
        isSkip = true;
    }
}
