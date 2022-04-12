using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserDeckData : MonoBehaviour
{
<<<<<<< Updated upstream
    public SaveDataSO saveData;
    private CardDeckSO cardDeck;
=======
    public SaveDataSO saveData; //���̺� ������(ī�� ����, ��������) 
    public CardDeckSO standardcardDeck; //���� ������ 
    public CardDeckSO deckList; //ī�� ������
    
    /// <summary>
    /// ī�嵦 �����͸� �������ݴϴ� 
    /// </summary>
    [ContextMenu("SetCardData")]
    public void SetCardData()
    {
        //���������� json���� �о��
        JsonToData();

        //ī�� ������ �ʱ�ȭ
        deckList.cardDatas.Clear();

        for(int i = 0; i < saveData.userSaveData.unitSaveDatas.Count; i++)
        {
            SaveData saveDataobj = saveData.userSaveData.unitSaveDatas[i];
            //������ Ÿ���� ���̺굥���Ϳ� ��� ���� ���� ������ ã��
            CardData cardDataobj = standardcardDeck.cardDatas.Find(x => x.cardType == saveDataobj._cardType 
                                            && x.unitData.unitType == saveDataobj._unitType
                                            && x.strategyData.starategyType == saveDataobj._strategicType);

            //���̺굥������ ������ŭ ��ġ�� �����ϰ� ���ο� ī�嵥���ͷ� ����� �޾� ������Ʈ�� �߰�
            deckList.cardDatas.Add(cardDataobj.DeepCopy(saveDataobj._level));

        }
    }
>>>>>>> Stashed changes

    string json; 
    /// <summary>
    /// ���� ���̺� �����͸� Jsonȭ ���� �����Ѵ�
    /// </summary>
    [ContextMenu("DataToJson")]
    public void DataToJson()    
    {
<<<<<<< Updated upstream
         json = JsonUtility.ToJson(saveData.userSaveData, false);
=======
        //�����͸� json���� ��ȯ
         string json = JsonUtility.ToJson(saveData.userSaveData, true);

>>>>>>> Stashed changes
        //json�� �����Ѵ� ���Ϸ� ����Ƽ���������ʿ�
        string fileName = "saveData";
        string path = Application.dataPath + "/" + fileName + ".json";
        File.WriteAllText(path, json); 
    }


    /// <summary>
    /// jsonȭ ��Ų 
    /// </summary>
    [ContextMenu("JsonToData")]
    public void JsonToData()
    {
<<<<<<< Updated upstream
        saveData.userSaveData = JsonUtility.FromJson<UserSaveData>(json);
=======
        //json ���� �ҷ�����
        string path = File.ReadAllText(Application.dataPath + "/saveData.json");    
        //json ������ ������ üũ
        if(path == null)
        {
            return;
        }
        
        //���������Ϳ� ����
        saveData.userSaveData = JsonUtility.FromJson<UserSaveData>(path);
>>>>>>> Stashed changes
    }
}
