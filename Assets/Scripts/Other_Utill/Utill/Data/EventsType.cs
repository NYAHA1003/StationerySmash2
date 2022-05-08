﻿namespace Utill.Data
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
        ActivePencilCaseDescription = 50, //덱에서 필통 정보창을 연다
        ChangePencilCase = 51, //필통을 바꾼다
        
        // 세팅 부분
        ActiveProfileImgPn = 100, // 프로필 이미지 패널 활성화 
        ChangeProfileImage, // 유저 이미지 변경 
        ChabgeUserName, // 유저 닉네임 변경 
        DeckSetting, // 덱 세팅
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
    }
}

