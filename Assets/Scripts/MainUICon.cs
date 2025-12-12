using System;
using UnityEngine;
using UnityEngine.UIElements;   //UI Toolkit 사용하기 위해

public class MainUICon : MonoBehaviour
{
    //public UIDocument document;
    UIDocument document;

    //UI 요소
    Label _titleLabel;
    Button _StartButton;
    Button _StopButton;
    Button _SettingButton;
    ProgressBar _hpBar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //UIDocument로부터 비쥬얼엘리먼트 루트 가져오기
        //document = GetComponent<UIDocument>();
        //VisualElement root = document.rootVisualElement;
        
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        //각 UI요소 찾기
        _titleLabel = root.Q<Label>("Title");
        _StartButton = root.Q<Button>("Start");
        _StopButton = root.Q<Button>("Stop");
        _SettingButton = root.Q<Button>("Setting");
        _hpBar = root.Q<ProgressBar>("HPBar");

        //제대로 찾았는지 확인
        //if(_titleLabel != null) print(_titleLabel.text + "찾았다");
        CheckUIElement(_titleLabel, "타이틀 라벨");
        CheckUIElement(_StartButton, "스타트 버튼");
        CheckUIElement(_SettingButton, "세팅 버튼");
        CheckUIElement(_StopButton, "스탑 버튼");
        CheckUIElement(_hpBar, "체력바");

        //UIToolkit은 모든 종류의 UI이벤트 처리가 가능하도록 콜백함수를 제공

        //마우스 이벤트 (클릭 : active, 호버 " hober
        //Endter(UI위에 있을 때) ,leave(UI에서 벗어날 때) ,down(클릭했을 때)
        _titleLabel.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
        //클릭 이벤트
        //_StartButton.RegisterCallback<ClickEvent>(OnClick);

        
    }


    private void OnMouseEnter(MouseEnterEvent evt)
    {

    }

    void CheckUIElement(VisualElement element, string name)
    {
        if (element != null) print(name + "찾음");
        else print(name + "못찾음");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
