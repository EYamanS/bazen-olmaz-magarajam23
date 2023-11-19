using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class OpenAITest : MonoBehaviour
{
    private string apiKey = "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";
    public string assistantToTalk;
    public Action<string> onRecieveMessage;

    [Button]
    public void ClearVals()
    {
        threadId = null;
        lastRunId = null;
        lastStepId = null;
        lastMessageId = null;
    }

    private void Awake()
    {
        ClearVals();
        StartCoroutine(CreateThread());
    }

    [ReadOnly]
    public string threadId;
    [ReadOnly]
    public string lastRunId;
    [ReadOnly]
    public string lastStepId;
    [ReadOnly]
    public string lastMessageId;

    [Button("Send Message")]
    public void SendDialog(string message)
    {
        StopAllCoroutines();
        StartCoroutine(SendDialogMessage(message));
    }

    public IEnumerator SendDialogMessage(string message)
    {
        yield return AddMessageToThread(message);
        yield return CreateRun();
        yield return GetLastStepMessage();
    }

    private IEnumerator GetLastStepMessage()
    {
        yield return GetLastRunStep();
        yield return RetrieveRunStep();
        yield return GetLastMessage();
    }


    private IEnumerator CreateThread()
    {

        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/threads", "POST");

        // Set headers
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        request.SetRequestHeader("OpenAI-Beta", "assistants=v1");

        // Create a dictionary to represent the JSON structure
        Dictionary<string, object> emptyJsonDict = new Dictionary<string, object>();

        var jsonStr = "";
        //Debug.Log(jsonStr);
        byte[] jsonData = System.Text.Encoding.UTF8.GetBytes(jsonStr);

        request.uploadHandler = new UploadHandlerRaw(jsonData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        //Debug.Log(request.result);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            //Debug.Log("Request successful!");
            // Handle response here if needed
            //Debug.Log("Response: " + request.downloadHandler.text);

            Dictionary<string, object> resultDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.downloadHandler.text);
            string thread_id = (string)resultDict["id"];
            threadId = thread_id;
        }
    }
    private IEnumerator AddMessageToThread(string message)
    {
        // https://api.openai.com/v1/threads/{thread_id}/messages

        UnityWebRequest request = new UnityWebRequest($"https://api.openai.com/v1/threads/{threadId}/messages", "POST");

        // Set headers
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        request.SetRequestHeader("OpenAI-Beta", "assistants=v1");

        // Create a dictionary to represent the JSON structure
        Dictionary<string, object> jsonDict = new Dictionary<string, object>();
        jsonDict.Add("role", "user");
        jsonDict.Add("content", message);


        var jsonStr = JsonConvert.SerializeObject(jsonDict);
        //Debug.Log(jsonStr);
        byte[] jsonData = System.Text.Encoding.UTF8.GetBytes(jsonStr);

        request.uploadHandler = new UploadHandlerRaw(jsonData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        //Debug.Log(request.result);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            //Debug.Log("Request successful!");
            // Handle response here if needed
            //Debug.Log("Response: " + request.downloadHandler.text);
        }
    }
    private IEnumerator CreateRun()
    {
        // https://api.openai.com/v1/threads/{thread_id}/runs

        UnityWebRequest request = new UnityWebRequest($"https://api.openai.com/v1/threads/{threadId}/runs", "POST");

        // Set headers
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        request.SetRequestHeader("OpenAI-Beta", "assistants=v1");

        // Create a dictionary to represent the JSON structure
        Dictionary<string, object> jsonDict = new Dictionary<string, object>();
        jsonDict.Add("assistant_id", assistantToTalk);


        var jsonStr = JsonConvert.SerializeObject(jsonDict);
        //Debug.Log(jsonStr);
        byte[] jsonData = System.Text.Encoding.UTF8.GetBytes(jsonStr);

        request.uploadHandler = new UploadHandlerRaw(jsonData);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        //Debug.Log(request.result);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            //Debug.Log("Run Request successful!");
            // Handle response here if needed
            //Debug.Log("Response: " + request.downloadHandler.text);

            Dictionary<string, object> resultDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.downloadHandler.text);
            string run_id = (string)resultDict["id"];
            lastRunId = run_id;
        }
    }
    private IEnumerator GetLastRunStep()
    {
        // https://api.openai.com/v1/threads/{thread_id}/runs/{run_id}/steps

        UnityWebRequest request = new UnityWebRequest($"https://api.openai.com/v1/threads/{threadId}/runs/{lastRunId}/steps", "GET");

        // Set headers
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        request.SetRequestHeader("OpenAI-Beta", "assistants=v1");

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        //Debug.Log(request.result);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        else
        {
            //Debug.Log("Run Request successful!");
            // Handle response here if needed
            //Debug.Log("Response: " + request.downloadHandler.text);

            Dictionary<string, object> resultDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.downloadHandler.text);

            lastStepId = (string)resultDict["last_id"];

            if (string.IsNullOrEmpty(lastStepId))
            {
                yield return new WaitForSeconds(.2f);
                yield return GetLastRunStep();
            }
        }
    }
    private IEnumerator RetrieveRunStep()
    {
        // https://api.openai.com/v1/threads/{thread_id}/runs/{run_id}/steps/{step_id}

        UnityWebRequest request = new UnityWebRequest($"https://api.openai.com/v1/threads/{threadId}/runs/{lastRunId}/steps/{lastStepId}", "GET");

        // Set headers
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        request.SetRequestHeader("OpenAI-Beta", "assistants=v1");

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        //Debug.Log(request.result);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            //Debug.Log("Run Request successful!");
            // Handle response here if needed
            //Debug.Log("Response: " + request.downloadHandler.text);

            Dictionary<string, object> resultDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.downloadHandler.text);
            var fetchedResult = false;

            try
            {
                //Debug.Log(resultDict["step_details"]);
                var stepDetails = JsonConvert.DeserializeObject<StepDetails>(resultDict["step_details"].ToString());
                lastMessageId = stepDetails.message_creation.message_id;
                fetchedResult = true;
            }

            catch (System.Exception ex)
            {
                fetchedResult = false;
            }

            if (!fetchedResult)
            {
                yield return new WaitForSeconds(.5f);
                yield return RetrieveRunStep();
            }
        }
    }
    private IEnumerator GetLastMessage()
    {
        // https://api.openai.com/v1/threads/{thread_id}/messages/{message_id}

        UnityWebRequest request = new UnityWebRequest($"https://api.openai.com/v1/threads/{threadId}/messages/{lastMessageId}", "GET");

        // Set headers
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        request.SetRequestHeader("OpenAI-Beta", "assistants=v1");

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        //Debug.Log(request.result);

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            //Debug.Log("Run Request successful!");
            // Handle response here if needed
            //Debug.Log("Response: " + request.downloadHandler.text);

            Dictionary<string, object> resultDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.downloadHandler.text);
            Content[] messageContent = JsonConvert.DeserializeObject<Content[]>(resultDict["content"].ToString());
            var response = messageContent[0].text.value;

            if (string.IsNullOrEmpty(response))
            {
                yield return new WaitForSeconds(.2f);
                yield return GetLastMessage();
            }
            else
            {
                onRecieveMessage?.Invoke(response);
            }
        }
    }

}


/*
[System.Serializable]
public struct Message
{
    public string role;
    public string content;
    public Message(string content)
    {
        this.role = "user";
        this.content = content;
    }
}
*/

[System.Serializable]
public struct Content
{
    public string type;
    public MessageText text;
}

[System.Serializable]
public struct MessageText
{
    public string value;
    public object annotations;
}


[System.Serializable]
public class StepDetails
{
    public string type { get; set; }
    public MessageCreation message_creation { get; set; }
}

[System.Serializable]
public class MessageCreation
{
    public string message_id { get; set; }
}