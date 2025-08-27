using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text;

public class HighScoreManager : MonoBehaviour
{
    private static string highScoreUrl = "https://matemathweb.com/wp-json/matemath-api/v1/get-top-scores";
    private static string saveScoreUrl = "https://matemathweb.com/wp-json/matemath-api/v1/insert-score-boom";
    private static string getUserUrl = "https://matemathweb.com/wp-json/matemath-api/v1/get-user";

    private static string username = "angel.larreategui@matemathweb.com";
    private static string password = "@Angel_0212";

    // Verifica el inicio de sesión del usuario
    

    // Obtiene el nombre de usuario por ID de usuario
    // Obtiene el nombre de usuario por ID de usuario
    public static async Task<User> GetUserById(string idUser)
    {
        string url = $"{getUserUrl}?id_user={idUser}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        // Añadir autenticación básica
        string auth = username + ":" + password;
        string encodedAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        request.SetRequestHeader("Authorization", "Basic " + encodedAuth);

        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseBody = request.downloadHandler.text;
            Debug.Log($"GetUserById response: {responseBody}");
            User jsonResponse = JsonUtility.FromJson<User>(responseBody);
            return jsonResponse;
        }
        else
        {
            Debug.LogError($"GetUserById request failed. Error: {request.error}");
            Debug.LogError($"Response content: {request.downloadHandler.text}");
            return null;
        }
    }

    // Guarda el puntaje máximo del jugador según la dificultad
    public static async Task<bool> SaveHighScore(int id_usuario, string id_niveles_juegos, int puntaje)
    {
        // Crear una instancia de HighScoreData
        HighScoreData highScoreData = new HighScoreData
        {
            id_usuario = id_usuario,
            id_niveles_juegos = id_niveles_juegos,
            puntaje = puntaje
        };

        // Convertir el objeto a JSON
        string jsonData = JsonUtility.ToJson(highScoreData);
        Debug.Log($"Sending high score data: {jsonData}");

        var request = new UnityWebRequest(saveScoreUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        string auth = username + ":" + password;
        string encodedAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        request.SetRequestHeader("Authorization", "Basic " + encodedAuth);

        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("High score saved successfully.");
            return true;
        }
        else
        {
            Debug.LogError("Failed to save high score. Error: " + request.error);
            Debug.LogError("Response content: " + request.downloadHandler.text);
            return false;
        }
    }

    // Obtiene los puntajes más altos de una dificultad
    public static async Task<List<HighScoreEntry>> GetHighScores(int id_juego, string nivel_juego)
    {
        string url = $"{highScoreUrl}?id_juego={id_juego}&nivel_juego={nivel_juego}";
        Debug.Log($"Requesting high scores with URL: {url}");

        var request = new UnityWebRequest(url, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();

        string auth = username + ":" + password;
        string encodedAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        request.SetRequestHeader("Authorization", "Basic " + encodedAuth);

        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseBody = request.downloadHandler.text;
            Debug.Log("Response from API: " + responseBody);
            List<HighScoreEntry> highScoreList = JsonUtility.FromJson<HighScoreList>("{\"highScores\":" + responseBody + "}").highScores;

            if (highScoreList == null || highScoreList.Count == 0)
            {
                Debug.Log("No high scores found.");
                return new List<HighScoreEntry>(); // Retornar una lista vacía
            }

            // Mensajes de depuración para cada entrada de high score
            foreach (var entry in highScoreList)
            {
                Debug.Log($"Position: {entry.puesto}, User: {entry.nickname_user}, Score: {entry.puntaje}");
            }

            return highScoreList;
        }
        else
        {
            Debug.LogError("Failed to get high scores from API. Error: " + request.error);
            Debug.LogError("Response content: " + request.downloadHandler.text);
            return new List<HighScoreEntry>(); // Retornar una lista vacía
        }
    }

    // Obtiene el puntaje más alto de un usuario específico
    public static async Task<int> GetUserHighScore(int id_juego, string user_nickname, string nivel_juego)
    {
        // Obtener la lista de puntajes desde la API
        List<HighScoreEntry> highScoreList = await GetHighScores(id_juego, nivel_juego);

        if (highScoreList != null && highScoreList.Count > 0)
        {
            // Buscar el puntaje del usuario específico en la lista
            foreach (var entry in highScoreList)
            {
                if (entry.nickname_user == user_nickname)
                {
                    Debug.Log($"Found user nickname: {entry.nickname_user}, Score: {entry.puntaje}");
                    return entry.puntaje;
                }
            }
            // Si no se encuentra el usuario, imprimir un mensaje de depuración
            Debug.Log($"User nickname {user_nickname} not found in high scores.");
        }
        else
        {
            Debug.LogError("Failed to get high scores from API or no high scores found.");
        }

        return 0; // Retornar 0 si no se encuentra el puntaje del usuario
    }


    public static async Task<bool> AumentarAlphaCoins(string username, int points)
    {
        

        string url = "https://matemathweb.com/wp-json/perfil-api/v1/update-points/";
        var requestData = new UpdatePointsRequest
        {
            user_login = username,
            points = points,
            action = "agregar"
        };

        string jsonData = JsonUtility.ToJson(requestData);
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            SetBasicAuthHeader(request);

            var operation = request.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Alpha points disminuidos para usuario: {username}");
                // Reactivar el EventSystem
               
                return true;
            }
            else
            {
                Debug.LogError($"Error al disminuir alpha points para {username}: {request.error}");
                // Reactivar el EventSystem
                return false;
            }
        }

    }

    private static void SetBasicAuthHeader(UnityWebRequest request)
    {
        string authInfo = $"{username}:{password}";
        string encodedAuth = Convert.ToBase64String(Encoding.ASCII.GetBytes(authInfo));
        request.SetRequestHeader("Authorization", "Basic " + encodedAuth);
    }
}



[System.Serializable]
public class HighScoreData
{
    public int id_usuario;
    public string id_niveles_juegos;
    public int puntaje;
}

[System.Serializable]
public class LoginData
{
    public string user_login;
    public string user_pass;
}

[System.Serializable]
public class User
{
    public int id_user;
    public string user_login;
    public string user_nickname;
}

[System.Serializable]
public class ApiResponse
{
    public string status;
    public User user;
}

[System.Serializable]
public class HighScoreEntry
{
    public int puesto;
    public string nickname_user;
    public int puntaje;
}

[System.Serializable]
public class HighScoreList
{
    public List<HighScoreEntry> highScores;
}

[Serializable]
    public class UpdatePointsRequest
    {
        public string user_login;
        public int points;
        public string action;
    }