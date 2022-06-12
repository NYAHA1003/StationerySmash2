namespace Utill.Data
{
    public enum EventsType
    {
        //버튼

        //메인 부분 
        ActiveDeck = 0, //덱 활성화 
        ActiveCardDescription , // 카드 설명 활성화
        ActiveSetting, //세팅 패널 활성화 
        MoveShopPn, // 상점패널 좌우 이동 (인덱스 변경)
        MoveMainPn, // 메인패널 상하 이동 (인덱스 변경) 
        CloaseAllPn, // 모든 패널 비활성화 
        SetOriginShopPn, //상점 패널 초기화 
        UpdateHaveAndEquipDeck, //보유 덱과 장착 덱 초기화
        UpdateHaveAndEquipPCDeck, // 보유 필통 덱 초기화
        ChangePCAndDeck, //덱에서 필통과 덱을 전환함
        SetPCInfoPanel = 50, //덱에서 필통 정보창에 정보를 넣는다
        ChangePencilCase = 51, //필통을 바꾼다
        ActivePencilCaseDescription, // 덱에서 필통 정보창 활성화
        UndoStack = 65, // 스택에 넣어둘 패널 활성화 해줄 버튼  

        CloseGacha = 75, // 뽑기하고 창 닫기
        CheckItem, // 뽑기한 아이템 하나씩 확인하기
        CheckCost, // 돈이 충분한가 확인 
        StartGacha, // 뽑기 버튼 클릭시 뽑기 시작 
        SkipAnimation, // 뽑기 애니메이션 스킵 
        ResetDailyShop, // 일일상점리셋
        ActiveNextBtn, // 뽑기 다음 버튼 활성화 

        // 세팅 부분
        ActiveProfileImgPn = 100, // 프로필 이미지 패널 활성화 
        ChangeProfileImage, // 유저 이미지 변경 
        ChabgeUserName, // 유저 닉네임 변경 
        ActiveButtonComponent, // 버튼 컴포넌트의 버튼 타입의 버튼 활성화
        MoveCredit, // 크레딧 밑으로 움직임  
        
        //스테이지 부분
        ActiveChapterPn = 200, //챕터 패널 활성화

        // 배틀 부분 
        CostUp = 300, // 코스트 증가
        PencilCaseAbility, // 필통 능력
        Pause, // 일시 정지 
        LoadMainScene, // 메인씬 로드 
        LoadBattleScene, // 배틀씬 로드 
        NextExplain, // 다음 튜토리얼 설명 
        SetTutorial, // 버튼 클릭시 어떤튜토리얼 나올지 

        ClearEvents, // 이벤트 딕셔너리 초기화 

    }
}

