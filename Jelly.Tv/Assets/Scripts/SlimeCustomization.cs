using System.Collections;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using UnityEngine;

public class SlimeCustomization : MonoBehaviour
{
    [System.Serializable]
    public class SlimeBody
    {
        [SerializeField] public Sprite shapeSprite;
        [SerializeField] public Vector3 facePos;
    }
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
    SlimeBody[] m_slimeShapes = null;

    private string[] m_faceNames;
    private string[] m_shapeNames;

    private Dictionary<string, Sprite> m_faceDictionary = new Dictionary<string, Sprite>();
    private Dictionary<string, SlimeBody> m_shapeDictionary = new Dictionary<string, SlimeBody>();

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
        for (int i = 0; i < m_slimeShapes.Length; i++)
        {
            string shapeType = m_slimeShapes[i].shapeSprite.name.Replace("slime_shape_", "");
            m_shapeNames[i] = shapeType;
        }
    }

    public void SetUpTwitchCommands()
    {
        for (int i = 0; i < m_slimeFaces.Length; i++)
        {
            m_faceDictionary.Add(m_faceNames[i], m_slimeFaces[i]);
        }

        for (int i = 0; i < m_slimeShapes.Length; i++)
        {
            m_shapeDictionary.Add(m_shapeNames[i], m_slimeShapes[i]);
        }

        TwitchCommand faceCommand = new TwitchCommand("face", SwapSlimeFaceSprite);
        TwitchClient.CommandManager.Commands.Add(faceCommand);
        TwitchCommand shapeCommand = new TwitchCommand("shape", SwapSlimeShapeSprite);
        TwitchClient.CommandManager.Commands.Add(shapeCommand);

        //this is for color
        TwitchCommand colorCommand = new TwitchCommand("color", ChangeSlimeShapeColor);
        TwitchClient.CommandManager.Commands.Add(colorCommand);

        TwitchCommand shapeInfoCommand = new TwitchCommand("infoshape", ChangeSlimeShapeColor);
        TwitchClient.CommandManager.Commands.Add(shapeInfoCommand);

        TwitchCommand faceInfoCommand = new TwitchCommand("infoface", ChangeSlimeShapeColor);
        TwitchClient.CommandManager.Commands.Add(faceInfoCommand);
    }

    public void ShapeInfo(object sender, OnChatCommandReceivedArgs e)
    {
        TwitchClient.Instance.Client.SendMessage(TwitchClient.Instance.Client.JoinedChannels[0], 
            "Slime Shape Commands(use !shape [shape]): basic, big, round, square, tall");
    }

    public void FaceInfo(object sender, OnChatCommandReceivedArgs e)
    {
        TwitchClient.Instance.Client.SendMessage(TwitchClient.Instance.Client.JoinedChannels[0],
            "Slime Face Commands(use !face [face]): basic, baby, bastard, kissy, nya, sad, sleepy, smug");
    }

    public void SwapSlimeFaceSprite(object sender, OnChatCommandReceivedArgs e)
    {
        string user = e.Command.ChatMessage.UserId;
        string faceTypeInput = e.Command.ArgumentsAsString;
        for (int i = 0; i < m_faceNames.Length; i++)
        {
            if (faceTypeInput == m_faceNames[i])
            {
                PlayerManager.Player player = PlayerManager.Instance.PlayerDictioary[user];
                Slime slime = player.Slime;
               
                slime.FaceSpriteRenderer.sprite = m_slimeFaces[i];
                player.SlimeFace = faceTypeInput;
            }
        }
        PlayerManager.Instance.SavePlayers();
    }

    public void SwapSlimeShapeSprite(object sender, OnChatCommandReceivedArgs e)
    {
        string user = e.Command.ChatMessage.UserId;
        string shapeTypeInput = e.Command.ArgumentsAsString;
        for (int i = 0; i < m_shapeNames.Length; i++)
        {
            if (shapeTypeInput == m_shapeNames[i])
            {
                PlayerManager.Player player = PlayerManager.Instance.PlayerDictioary[user];
                Slime slime = player.Slime;
                slime.ShapeSpriteRenderer.sprite = m_slimeShapes[i].shapeSprite;
                slime.FaceSpriteRenderer.gameObject.transform.localPosition = m_slimeShapes[i].facePos;
                player.SlimeShape = shapeTypeInput;
            }
        }
        PlayerManager.Instance.SavePlayers();
    }

    public void ChangeSlimeShapeColor(object sender, OnChatCommandReceivedArgs e)
    {
        string user = e.Command.ChatMessage.UserId;
        string colorInput = e.Command.ArgumentsAsString;
        Color slimeColor;
        ColorUtility.TryParseHtmlString(colorInput, out slimeColor);

        if(slimeColor != null)
        {
            PlayerManager.Player player = PlayerManager.Instance.PlayerDictioary[user];
            Slime slime = player.Slime;
            slime.ShapeSpriteRenderer.color = slimeColor;
            player.SlimeColor = colorInput;
        }

        PlayerManager.Instance.SavePlayers();
    }

    public void SetSlime(string face, string shape, Slime slime, string color)
    {
        if (m_shapeDictionary.ContainsKey(shape))
        {
            slime.ShapeSpriteRenderer.sprite = m_shapeDictionary[shape].shapeSprite;
            slime.FaceSpriteRenderer.gameObject.transform.localPosition = m_shapeDictionary[shape].facePos;
        }
        if(m_faceDictionary.ContainsKey(face)) slime.FaceSpriteRenderer.sprite = m_faceDictionary[face];
        Color colorSlime;
        ColorUtility.TryParseHtmlString(color, out colorSlime);
        slime.ShapeSpriteRenderer.color = colorSlime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
