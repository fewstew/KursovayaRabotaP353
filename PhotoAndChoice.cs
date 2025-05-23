using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using UnityEditor;


public class PhotoAndChoice : MonoBehaviour
{
    //Переменная скорости пневматического сдувателя
    float TrashSpeed = 0f;
    //Переменная выбора отбраковки
    public float Choice;
    // Путь к исполняемому файлу Python
    string pythonPath = "C:/Users/fewst/python313/pythonw.exe";
    // Путь к Python-скрипту
    string scriptPath = "C:/Users/fewst/Desktop/Photo_by_Camera/HELP.py";
    // Путь для сохранения фотографии
    string imagePath = "C:/Users/fewst/Desktop/Photo_by_Camera/image.png";
    // Путь к файлу с результатом
    string resultFilePath = "C:/Users/fewst/Desktop/Photo_by_Camera/result.txt";
    // Публичное поле для привязки камеры в Unity
    Camera cameraToUse;
    //Старт конвейера и поиск камеры из всех объектов
    void Start()
    {
        GameObject Camera = GameObject.FindGameObjectWithTag("Photo");
        if (Camera != null)
        {
            cameraToUse = Camera.GetComponent<Camera>();
        }
        else
        {
            print("Зачем поменяли тег?");
        }
        WTF();
    }
    //Функция пневмотолкателя
    void Update()
    {
        transform.position += new Vector3(TrashSpeed, 0, 0);
    }
    //Функции изменения скорости в программе для конвейера
    void WTF() => GameObject.FindGameObjectWithTag("Con1").GetComponent<Conveyormovement>().speedC = 1f;
    void WTF1() => GameObject.FindGameObjectWithTag("Con1").GetComponent<Conveyormovement>().speedC = 0f;
    //коллизия для фото, остановки падения и остановки конвейера
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
    //корутина задержки времени и работы с функциями выбора и фото
    IEnumerator Delay()
    {
        WTF1();
        yield return new WaitForSeconds(1);
        TakePhoto();
        yield return new WaitForSeconds(3);
        // Отправляем сигнал в Unity
        ReadResult();
        Trash();
        yield return new WaitForSeconds(1);
        WTF();
    }
    //Программа для отбраковки
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
    //Программа для фотографии
    void TakePhoto()
    {
        // Создаем временный рендер-текстур для сохранения снимка экрана
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraToUse.targetTexture = renderTexture;
        Texture2D photo = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        cameraToUse.Render();
        RenderTexture.active = renderTexture;
        photo.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        cameraToUse.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        // Сохраняем фотографию на диск
        byte[] bytes = photo.EncodeToPNG();
        File.WriteAllBytes(imagePath, bytes);
     // Запускаем Python-скрипт для обработки фотографии
        Process process = new Process();
        process.StartInfo.FileName = pythonPath;
        process.StartInfo.Arguments = scriptPath;
        // Выводим команду в консоль Unity
        process.StartInfo.UseShellExecute = false;
        process.Start();
        // Ожидаем завершения выполнения Python-скрипта
        process.WaitForExit();
        //Альтернативный вариант запуска скрипта
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
    //Программа считывания данных из txt файла
        void ReadResult()
    {
        // Читаем результат обработки фотографии из файла
        string result = File.ReadAllText(resultFilePath);
        // Отправляем сигнал в Unity
        ProcessUnitySignal(result);
    }

    //Программа обработки результата считывания txt файла
        void ProcessUnitySignal(string result)
        {
        // Обработка полученного сигнала в Unity
        // Добавьте необходимый код для выполнения действий в Unity на основе результата
        Renderer renderer = GetComponent<Renderer>();

        if (result == "0")
        {
            Choice = 0;
            print("Брак!");
        }
        else if (result == "1")
        {
            Choice = 1;
            print("Плата отличного качества!");
        }
        else
        {
            print("Плата не считалась");
        }
    }
}
