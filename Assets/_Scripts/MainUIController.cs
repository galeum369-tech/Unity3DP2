using System;
using UnityEngine;
using UnityEngine.UIElements;   //UI Toolkit 사용하기 위해서

public class MainUIController : MonoBehaviour
{
    //public UIDocument document;
    //UIDocument document;

    //UI 요소
    Label _titleLabel;
    Button _startButton;
    Button _stopButton;
    Button _settingButton;
    ProgressBar _hpBar;

    private void OnEnable()
    {
        print("OnEnable");

        //콜백 이벤트 함수 등록
        //UI 요소들을 찾을 수 없으니 UI 초기화는 Awake에서 해야 한다 
        _titleLabel.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
        _startButton.clicked += OnStartButtonClicked;
        _stopButton.clicked += OnStopButtonClicked;
        _hpBar.RegisterCallback<KeyDownEvent>(OnKeyDown);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        print("Start");

        //UIDocument로부터 비쥬얼엘리먼트 루트 가져오기
        //document = GetComponent<UIDocument>();
        //VisualElement root = document.rootVisualElement;
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        //각 UI 요소 찾기
        _titleLabel = root.Q<Label>("Title");
        _startButton = root.Q<Button>("Start");
        _stopButton = root.Q<Button>("Stop");
        _settingButton = root.Q<Button>("Setting");
        _hpBar = root.Q<ProgressBar>("HpBar");

        //제대로 찾았는지 확인해보자
        //if (_titleLabel != null) print(_titleLabel.text + "찾았다");
        //CheckUIElement(_titleLabel, "타이틀 라벨");
        //CheckUIElement(_startButton, "스타트 버튼");
        //CheckUIElement(_settingButton, "세팅 버튼");
        //CheckUIElement(_stopButton, "종료 버튼");
        //CheckUIElement(_hpBar, "체력바");

        //UIToolkit은 모든 종류의 UI이벤트 처리가 가능하도록 콜백함수를 제공한다

        //마우스 이벤트 (클릭 :active, 호버 :hover)
        //Enter(UI위에 있을때), leave(UI에서 벗어났을때), down(클릭했을때)
        //_titleLabel.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
        //클릭 이벤트
        //_startButton.RegisterCallback<ClickEvent>(OnClick);
        //_stopButton.clicked += OnClicked;
        //키 이벤트
        //_hpBar.RegisterCallback<KeyDownEvent>(OnKeyDown);

        //RegisterCallback<T> 모든 종류 UI 이벤트 처리 가능
        //가장 일반적이로 강력한 이벤트 처리 방법
        //_stopButton.clicked += 함수이름
        //clicked += 이벤트 처리방식은 UI 버튼 요소에 특화된 속성으로 간편하고 직관적이다
        //관례적으로 UI의 버튼을 다룰때는 이방식을 더 선호 한다
        //내부적으로 RegisterCallback<T>를 사용하고 있다
        //두개중 성능상 어떤걸 사용해도 무방하다

        //이벤트 함수들은 메모리 누수 방지를 위해
        //반드시 콜백함수들을 해제를 시켜줘야 한다
        //보통 OnEnable() => 등록, OnDisable() => 해제
        //Start() => 등록, Destroy() => 해제
        //clicked += 등록시 -= 해제하고
        //RegisterCallback<T> 등록시 UnregisterCallback<T> 해제한다

        //콜백 이벤트 초기화
        ResisgerCallBack();

    }

    void ResisgerCallBack()
    {
        _titleLabel.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
        _titleLabel.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        _startButton.clicked += OnStartButtonClicked;
        _settingButton.clicked += OnSettingButtonClicked;
        _stopButton.clicked += OnStopButtonClicked;
        //_hpBar.RegisterCallback<KeyDownEvent>(OnKeyDown);
        _hpBar.RegisterCallback<KeyDownEvent>(OnKeyDownEvent);
    }

    private void OnSettingButtonClicked()
    {
        print("HP: " + _hpBar.value);
        _hpBar.value -= 1f;
        _hpBar.title = _hpBar.value.ToString();
    }

    private void OnKeyDownEvent(KeyDownEvent evt)
    {
        print("키다운");
        print(evt.keyCode.ToString());
    }


    /// <summary>
    /// UI 요소 확인용 함수
    /// </summary>
    /// <param name="evt"></param>
    void CheckUIElement(VisualElement element, string name)
    {
        if (element != null) print(name + " 찾았음");
        else print(name + "못 찾았음");
    }

    //타이틀
    private void OnMouseEnter(MouseEnterEvent evt)
    {
        _titleLabel.text = "마우스 치우라~";
    }

    private void OnMouseLeave(MouseLeaveEvent evt)
    {
        _titleLabel.text = "Title";
    }

    private void OnStartButtonClicked()
    {
        _titleLabel.text = "시작버튼 클릭";
    }

    private void OnStopButtonClicked()
    {
        _titleLabel.text = "종료버튼 클릭";
    }

    //체력바
    private void OnKeyDown(KeyDownEvent evt)
    {
        if (evt.keyCode == KeyCode.Escape)
        {
            print("아무키나 물러보자구 ~ 키다운 잘 되나?");
        }
        print(evt.keyCode);

    }


    private void OnDisable()
    {
        //콜백 이벤트 함수 해제
        _titleLabel.UnregisterCallback<MouseEnterEvent>(OnMouseEnter);
        _startButton.clicked -= OnStartButtonClicked;
        _stopButton.clicked -= OnStopButtonClicked;
        _hpBar.UnregisterCallback<KeyDownEvent>(OnKeyDown);
    }
}
