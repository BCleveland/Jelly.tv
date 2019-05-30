using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;

public class SlimeCustomization : MonoBehaviour
{
    /*
     * CURRENT SLIME FACES:
     * basic
     * baby
     * bastard
     * kissy
     * nya
     * sad
     * sleepy
     * smug
     */
    [Header("Slime Faces")]
    [SerializeField]
    Sprite[] m_slimeFaces = null;

    /*
     * CURRENT SLIME SHAPES:
     * basic
     * big
     * round
     * square
     * tall
     */
    [Header("Slime Shapes")]
    [SerializeField]
    Sprite[] m_slimeShapes = null;

    private string[] m_faceNames;
    private string[] m_shapeNames;

    private Dictionary<TwitchCommand, Sprite> m_faceDictionary = new Dictionary<TwitchCommand, Sprite>();
    private Dictionary<TwitchCommand, Sprite> m_shapeDictionary = new Dictionary<TwitchCommand, Sprite>();

    private TwitchClient TwitchClient = null;

    void Start()
    {
        TwitchClient = FindObjectOfType<TwitchClient>();

        SetupSlimeFaceNames();
        SetupSlimeShapeNames();

        SetUpTwitchCommands();
    }

    public void SetupSlimeFaceNames()
    {
        m_faceNames = new string[m_slimeFaces.Length];
        for (int i = 0; i < m_slimeFaces.Length; i++)
        {
            string faceType = m_slimeFaces[i].name.Replace("slime_face_", "");
            m_faceNames[i] = faceType;
        }
    }

    public void SetupSlimeShapeNames()
    {
        m_shapeNames = new string[m_slimeShapes.Length];
        for (int i = 0; i < m_slimeFaces.Length; i++)
        {
            string shapeType = m_slimeShapes[i].name.Replace("slime_face_", "");
            m_shapeNames[i] = shapeType;
        }
    }

    public void SetUpTwitchCommands()
    {
        for (int i = 0; i < m_slimeFaces.Length; i++)
        {
            TwitchCommand command = new TwitchCommand("face", SwapSlimeFaceSprite);
            m_faceDictionary.Add(command, m_slimeFaces[i]);
            if(TwitchClient != null)
            {
                TwitchClient.Commands.Add(command);
            }
        }

        for (int i = 0; i < m_slimeShapes.Length; i++)
        {
            TwitchCommand command = new TwitchCommand("shape", SwapSlimeShapeSprite);
            m_shapeDictionary.Add(command, m_slimeShapes[i]);
            if (TwitchClient != null)
            {
                TwitchClient.Commands.Add(command);
            }
        }
    }

    public void SwapSlimeFaceSprite(object sender, OnChatCommandReceivedArgs e)
    {
        string user = e.Command.ChatMessage.UserId;
        string faceTypeInput = e.Command.ArgumentsAsString;
        for (int i = 0; i < m_faceNames.Length; i++)
        {
            if (faceTypeInput == m_faceNames[i])
            {
                foreach (Slime slime in FindObjectsOfType<Slime>())
                {
                    if(slime.PlayerID == user)
                    {

                    }
                }
            }
        }
    }

    public void SwapSlimeShapeSprite(object sender, OnChatCommandReceivedArgs e)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
