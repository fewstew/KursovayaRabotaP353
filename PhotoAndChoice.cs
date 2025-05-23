using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEditor;


public class PhotoAndChoice : MonoBehaviour
{
    //���������� �������� ��������������� ���������
    float TrashSpeed = 0f;
    //���������� ������ ����������
    public float Choice;
    // ���� � ������������ ����� Python
    string pythonPath = "C:/Users/fewst/python313/pythonw.exe";
    // ���� � Python-�������
    string scriptPath = "C:/Users/fewst/Desktop/Photo_by_Camera/HELP.py";
    // ���� ��� ���������� ����������
    string imagePath = "C:/Users/fewst/Desktop/Photo_by_Camera/image.png";
    // ���� � ����� � �����������
    string resultFilePath = "C:/Users/fewst/Desktop/Photo_by_Camera/result.txt";
    // ��������� ���� ��� �������� ������ � Unity
    Camera cameraToUse;
    //����� ��������� � ����� ������ �� ���� ��������
    void Start()
    {
        GameObject Camera = GameObject.FindGameObjectWithTag("Photo");
        if (Camera != null)
        {
            cameraToUse = Camera.GetComponent<Camera>();
        }
        else
        {
            print("����� �������� ���?");
        }
        WTF();
    }
    //������� ���������������
    void Update()
    {
        transform.position += new Vector3(TrashSpeed, 0, 0);
    }
    //������� ��������� �������� � ��������� ��� ���������
    void WTF() => GameObject.FindGameObjectWithTag("Con1").GetComponent<Conveyormovement>().speedC = 1f;
    void WTF1() => GameObject.FindGameObjectWithTag("Con1").GetComponent<Conveyormovement>().speedC = 0f;
    //�������� ��� ����, ��������� ������� � ��������� ���������
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "sensor0")
        {
            WTF1();
        }
        if (other.gameObject.tag == "sensor2")
        {
            TrashSpeed = 0f;
        }
        if (other.gameObject.tag == "sensor1")
        {
            StartCoroutine(Delay());

        }
    }
    //�������� �������� ������� � ������ � ��������� ������ � ����
    IEnumerator Delay()
    {
        WTF1();
        yield return new WaitForSeconds(1);
        TakePhoto();
        yield return new WaitForSeconds(3);
        // ���������� ������ � Unity
        ReadResult();
        Trash();
        yield return new WaitForSeconds(1);
        WTF();
    }
    //��������� ��� ����������
    private void Trash()
    {
        if (Choice == 1)
        {
        TrashSpeed = 0f;
        }
        else if (Choice == 0)
        {
            TrashSpeed = 0.02f;
        }
    }
    //��������� ��� ����������
    void TakePhoto()
    {
        // ������� ��������� ������-������� ��� ���������� ������ ������
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraToUse.targetTexture = renderTexture;
        Texture2D photo = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        cameraToUse.Render();
        RenderTexture.active = renderTexture;
        photo.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        cameraToUse.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        // ��������� ���������� �� ����
        byte[] bytes = photo.EncodeToPNG();
        File.WriteAllBytes(imagePath, bytes);
     // ��������� Python-������ ��� ��������� ����������
        Process process = new Process();
        process.StartInfo.FileName = pythonPath;
        process.StartInfo.Arguments = scriptPath;
        // ������� ������� � ������� Unity
        process.StartInfo.UseShellExecute = false;
        process.Start();
        // ������� ���������� ���������� Python-�������
        process.WaitForExit();
        //�������������� ������� ������� �������
        /*
        string pythonPath = "C:/Users/il/Kursovaya/Assets/Photo/new_python_script.py";
        if (File.Exists(pythonPath))
        {
            Process process = new Process();
            process.StartInfo.FileName = pythonPath;
            process.StartInfo.UseShellExecute = true;
            process.Start();
            process.WaitForExit();
        }
        */
        //Process.Start("C:/Users/il/Desktop/Photo_by_Camera/HELP.py");
    }
    //��������� ���������� ������ �� txt �����
        void ReadResult()
    {
        // ������ ��������� ��������� ���������� �� �����
        string result = File.ReadAllText(resultFilePath);
        // ���������� ������ � Unity
        ProcessUnitySignal(result);
    }

    //��������� ��������� ���������� ���������� txt �����
        void ProcessUnitySignal(string result)
        {
        // ��������� ����������� ������� � Unity
        // �������� ����������� ��� ��� ���������� �������� � Unity �� ������ ����������
        Renderer renderer = GetComponent<Renderer>();

        if (result == "0")
        {
            Choice = 0;
            print("����!");
        }
        else if (result == "1")
        {
            Choice = 1;
            print("����� ��������� ��������!");
        }
        else
        {
            print("����� �� ���������");
        }
    }
}
