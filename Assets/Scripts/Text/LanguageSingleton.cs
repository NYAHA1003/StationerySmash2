using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class Lang
{
    public string lang, langLocalize; // lang���� �� ����� ex) Korean, langLocalize  ����� ����ȭ �̸��� ����� ���̴�. ex)�ѱ���
    public List<string> value = new List<string>(); // ����� Text value���� ����ش�.
}

public class LanguageSingleton : MonoSingleton<LanguageSingleton>
{
    const string langURL = "https://docs.google.com/spreadsheets/d/19NVshReriTZsf36cCWqUvFX6bhaWSGoDOb--1XZ0IVo/export?format=tsv&gid=2132424280"; //��� �����Ͱ� ����� ���� �������� ��Ʈ�� ��ũ
    public event Action LocalizeChanged = () => { };
    public event Action LocalizeSettingChanged = () => { };

    public int curLangIndex;    // ���� ����� �ε���
    public List<Lang> Langs;    // ��� ������ Ŭ������ ����Ʈ

    // InitLang �Լ������� �����س��� ��� �ε������� �ִٸ� �������� , ���ٸ� �⺻���(����)�� �ε��� ���� �����´�.
    void InitLang()
    {
        int langIndex = PlayerPrefs.GetInt("LangIndex", -1);
        int systemIndex = Langs.FindIndex(x => x.lang.ToLower() == Application.systemLanguage.ToString().ToLower());
        if (systemIndex == -1) systemIndex = 0;
        int index = langIndex == -1 ? systemIndex : langIndex;

        SetLangIndex(index); // ���� ������ �� SetLangIndex�� �Ű������� �־��ش� 
    }


    public void SetLangIndex(int index)
    {
        curLangIndex = index;   //initlang���� ���� ����� �ε��� ���� curLangIndex�� �־��� 
        PlayerPrefs.SetInt("LangIndex", curLangIndex);  //����
        LocalizeChanged();  //�ؽ�Ʈ�� ���� ���� ����
        LocalizeSettingChanged();   //����ٿ��� value����
    }


    [ContextMenu("��� ��������")]
    void GetLang()
    {
        StartCoroutine(GetLangCo());
    }

    IEnumerator GetLangCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(langURL); //�������� ��Ʈ�� url�� ��������
        yield return www.SendWebRequest();  //������ �� ���� ��� 
        SetLangList(www.downloadHandler.text);  //�������� ��Ʈ�� ������ ���� SetLangList�� �־��ش�.
    }

    void SetLangList(string tsv)
    {
        // ������ �迭 ����

        string[] row = tsv.Split('\n'); //�����̽��� ������ �� �з� 
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length; //���� �������� �� �з�
        string[,] Sentence = new string[rowSize, columnSize];   // ������ �迭 ����


        // ������ �迭�� ������ �־��ֱ�
        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++)
                Sentence[i, j] = column[j];
        }

        Langs = new List<Lang>();   //���ο� Langs ��ü �����ϰ�,

        for (int i = 0; i < columnSize; i++)
        {
            Lang lang = new Lang(); //�ϳ��� �� �ǹ��ϴ� ��ü ����
            lang.lang = Sentence[0, i]; //����� ���� �̸� 
            lang.langLocalize = Sentence[1, i]; //����� ����ȭ �̸�

            for (int j = 2; j < rowSize; j++) lang.value.Add(Sentence[j, i]);   //�ؽ�Ʈ value���� ��ü�� �־��ش�.
            Langs.Add(lang);    //������ ����ִ� ����Ʈ�� �߰�
        }
    }
}
