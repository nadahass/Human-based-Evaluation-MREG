//Future work: 
//consider bidirictional interaction
//consider the relationship between events e.g., grasp this block, put it on the green block
//increase the inference efffecency

using UnityEngine;
using System.Net;
using System.IO;
using System.Dynamic;
using System;
using System.Collections.Generic;
using System.Collections;
using VoxSimPlatform.SpatialReasoning;
using System.Timers;
using Newtonsoft.Json.Linq;
using ScriptInspector;
using VoxSimPlatform.Vox;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine.UI;
using UnityEditor.Recorder;
using UnityEditor;
using UnityEditor.Recorder.Input;

public class VideoRecorings : ModuleBase
{


    //Generation setting parameters

    string configration = "RedBlock1(-0.098242240,1.124870000,0.368312500) and RedBlock2(-0.511479800,1.124870000,0.253864300) and GreenBlock1(0.265574600,1.124870000,-0.446640600) and GreenBlock2(-0.111404000,1.124870000,-0.266982000) and BlueBlock1(0.179088900,1.124870000,0.237997900) and BlueBlock2(0.014448110,1.124870000,-0.159720900) and PinkBlock1(-0.511480500,1.224870000,0.253864600) and PinkBlock2(0.279113600,1.125561000,0.237998100) and YellowBlock1(-0.315363000,1.124870000,-0.126586800) and YellowBlock2(0.049953640,1.124870000,0.409480800)";
    string focusObj = "PinkBlock2";
    string output = "No speech , None , pointing only , (0.279113600: 1.125561000: 0.237998100)";
    string output_h = "No speech , None , pointing only , (0.279113600: 1.125561000: 0.237998100)";
    string instruction = "Generate a referring expression for an object.";
    string input;
    public GameObject behaviorController;
    public RelationTracker relationTracker;
    public Transform grabbableObjects;

    public double FocusTimerTime = 2000;
    System.Timers.Timer FocusTimer;

    public double TargetTimerTime = 30;
    System.Timers.Timer TargetTimer;

    bool FocusElapse = false;
    bool TargetElapse = false;
    List<string> describedBlocks = new List<string>();
    List<string> actions = new List<string>();

    Animator spriteAnimator1, spriteAnimator2, spriteAnimator3,
    spriteAnimator4, spriteAnimator5, spriteAnimator6, spriteAnimator7,
    spriteAnimator8, spriteAnimator9, spriteAnimator10;

    public Image focusCircle1;
    public static int focusTimeoutTime = 100;
    bool focusCirc = false;

    public float timePeriod = 2;
    public float height = 30f;
    public float startAngle;
    private float timeSinceStart;
    private Vector3 pivot;
    private string funword = "test";
    private RecorderController TestRecorderController;

    string tepBlock;

    string[] config;

    string[] focus;

    string[] diana_output;

    string[] human_output;

    bool[] visit;
    string[] diana_names;

    string[] human_names;

    RecorderController m_RecorderController;
    bool done = false;
    bool start = false;
    public bool m_RecordAudio = true;
    internal MovieRecorderSettings m_Settings = null;
    bool blueb = true;
    public GameObject blue1;
    public GameObject blue2;
    public GameObject green1;
    public GameObject green2;
    public GameObject red1;
    public GameObject red2;
    public GameObject yellow1;
    public GameObject yellow2;
    public GameObject pink1;
    public GameObject pink2;
    string name;
    public void Start()
    {
        base.Start();
        // Nada: added for relational REs Understanding 
        behaviorController = GameObject.Find("BehaviorController");
        // Nada: added for relational REs Understanding 
        relationTracker = behaviorController.GetComponent<RelationTracker>();
        //HumanMREGenerationTest(configration, output, focusObj);
        //StartRecorder();
        focusCircle1.enabled = false;
        //blue.enabled = false;
        spriteAnimator1 = focusCircle1.GetComponent<Animator>();
        spriteAnimator1.enabled = false;
        //StartRecording();


        config = new[]{configration1
            , configration2, configration3, configration4,
            configration5,configration6, configration7, configration8, configration9
        configration10, configration11, configration12, configration13, configration14,
            configration15, configration16, configration17, configration18, configration19,
            configration20, configration21, configration22, configration23,*/ configration24,
            configration25, configration26, configration27, configration28, configration29,
            configration30, configration31, configration32, configration33,configration34,
            configration35,configration36, configration37, configration38, configration39,
            configration40, configration41
        };

        //config = new[] { configration };

        focus = new[]{ focusObj1
            , focusObj2, focusObj3, focusObj4, focusObj5, focusObj6,
            focusObj7, focusObj8, focusObj9,focusObj10, focusObj11, focusObj12, focusObj13,
            focusObj14, focusObj15, focusObj16,focusObj17, focusObj18, focusObj19, focusObj20,
            focusObj21, focusObj22, focusObj23, focusObj24, focusObj25, focusObj26,*/
            focusObj27, focusObj28, focusObj29, focusObj30, focusObj31, focusObj32,focusObj33,
           focusObj34, focusObj35, focusObj36, focusObj37, focusObj38, focusObj39, focusObj40,
            focusObj41
         };

        //focus = new[] { focusObj };

        diana_output = new[]{ output1, output2, output3, output4, output5, output6,
            output7, output8, output9,output10, output11, output12, output13,
        output14, output15, output16,output17, output18, output19, output20,
        output21, output22, output23, output24, output25, output26,
        output27, output28, output29, output30, output31, output32, output33,
        output34, output35, output36, output37, output38, output39, output40,
        output41
        };

        //diana_output = new[] { output };

        human_output = new[]{output_h1
            , output_h2, output_h3, output_h4, output_h5, output_h6,
           output_h7, output_h8, output_h9, output_h10, output_h11, output_h12, output_h13,
            output_h14, output_h15, output_h16, output_h17, output_h18, output_h19, output_h20,
            output_h21, output_h22, output_h23, output_h24, output_h25, output_h26,
            output_h27, output_h28, output_h29, output_h30, output_h31, output_h32, output_h33,
            output_h34, output_h35, output_h36, output_h37, output_h38, output_h39, output_h40,
            output_h41
        };

        //human_output = new[] { output_h9};

        diana_names = new[]{
            "9-10 diana"
            "19-20 diana", "21-22 diana", "23-24 diana", "25-26 diana",
            "27-28 diana", "29-30 diana", "31-32 diana", "33-34 diana", "35-36 diana",
            "37-38 diana", "39-40 diana","41-42 diana", "43-44 diana", "45-46 diana",
            "47-48 diana", "49-50", "51-52 diana", "53-54 diana", "55-56 diana", "57-58 diana",
            "59-60 diana", "61-62 diana", "63-64 diana", "65-66 diana", "67-68 diana",
            "69-70 diana", "71-72 diana", "73-74 diana", "75-76 diana", "77-78 diana",
            "79-80 diana", "81-82 diana","83-84 diana", "85-86 diana","87-88 diana",
            "89-90 diana", "91-92 diana", "93-94 diana", "95-96 diana",
            "97-98 diana", "99-100 diana"
    };

        //diana_names = new[]{"93-94 diana"};

        human_names = new[]{"1-2 human", "3-4 human", "5-6 human", "7-8 human",
            "9-10 human", "11-12 human","13-14 human", "15-16 human", "17-18 human",
            "19-20 human", "21-22 human", "23-24 human", "25-26 human",
            "27-28 human", "29-30 human", "31-32 human", "33-34 human",
            "35-36 human", "37-38 human", "39-40 human",
            "41-42 human", "43-44 human", "45-46 human", "47-48 human", "49-50 human",
            "51-52 human", "53-54 human","55-56 human", "57-58 human", "59-60 human",
            "61-62 human","63-64 human", "65-66 human", "67-68 human",
            "69-70 human", "71-72 human", "73-74 human", "75-76 human", "77-78 human",
            "79-80 human", "81-82 human","83-84 human", "85-86 human","87-88 human",
            "89-90 human", "91-92 human", "93-94 human", "95-96 human",
            "97-98 human", "99-100 human"
        };

        //human_names = new[] { "17-18 human"};

        visit = new[]{ false, false, false, false, false, false, false, false, false, false, false, false,
          false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
          false,false,false,false,false,false,false,false,false,false,false};

        // visit = new[] { false };

        TargetTimer = new System.Timers.Timer(TargetTimerTime);
        TargetTimer.Enabled = false;
        TargetTimer.Elapsed += TargetTimeElapse;
     

    }

    public void Awake()
    {

    }

    [Obsolete]
    private void Update()
    {

        for (int i = 0; i < 1; i++)
        {
            if (visit[i] == false && TargetTimer.Enabled == false)
            {
                DianaMREGenerationTest(config[i], diana_output[i], focus[i]);
                HumanMREGenerationTest(config[i], human_output[i], focus[i]);
                name = diana_names[i];
                name = human_names[i];
                visit[i] = true;
                StartRecording();
            }
            else
            {
                Debug.Log("list is completed");
            }

            if (TargetElapse)
            {
                Stop();
                Debug.Log("recording time elapsed");
                TargetElapse = false;
                focusCircle1.enabled = false;
            }
        }

        // If target GREs time is elapsed 

        if (focusCirc == true)
        {
            focusCircle1.enabled = true;
            spriteAnimator1.enabled = true;

        }

    }

    public FileInfo OutputFile
    {
        get
        {
            var fileName = m_Settings.OutputFile + ".mov";
            return new FileInfo(fileName);
        }
    }

    void TargetTimeElapse(object sender, ElapsedEventArgs e)
    {
        TargetElapse = true;
        TargetTimer.Interval = TargetTimerTime;
        TargetTimer.Enabled = false;
    }

    void StartRecording()
    {
        Initialize();
        Debug.Log("Start recording");
    }

    internal void Initialize()
    {

        var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        m_RecorderController = new RecorderController(controllerSettings);

        var mediaOutputFolder = new DirectoryInfo(Path.Combine(Application.dataPath, "..", "SampleRecordings"));

        // Video
        m_Settings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        m_Settings.name = "My Video Recorder";
        m_Settings.Enabled = true;

        // This example performs an MP4 recording
        m_Settings.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
        m_Settings.VideoBitRateMode = VideoBitrateMode.High;

        m_Settings.ImageInputSettings = new GameViewInputSettings
        {
            OutputWidth = 1920,
            OutputHeight = 1080
        };

        m_Settings.AudioInputSettings.PreserveAudio = m_RecordAudio;

        // Simple file name (no wildcards) so that FileInfo constructor works in OutputFile getter.
        m_Settings.OutputFile = mediaOutputFolder.FullName + "/" + name;

        // Setup Recording
        controllerSettings.AddRecorderSettings(m_Settings);
        controllerSettings.SetRecordModeToManual();
        controllerSettings.FrameRate = 60.0f;

        RecorderOptions.VerboseMode = false;
        m_RecorderController.PrepareRecording();
        m_RecorderController.StartRecording();

        Debug.Log($"Started recording for file {OutputFile.FullName}");
    }


    void Stop()
    {
        m_RecorderController.StopRecording();
        Debug.Log("Stop recording");
    }

    public void HumanMREGenerationTest(string config, string output, string focus)
    {
        TargetTimer.Enabled = true;
        string[] splitoutput = output.Split(',');
        string newConfig = config.Replace(" and ", ":");
        string[] confi = newConfig.Split(':');

        // generate the scene config
        for (int i = 0; i < confi.Length; i++)
        {
            string[] blockPos = confi[i].Split('(');
            string block = blockPos[0];
            string position = blockPos[1];
            string[] pos = position.Split(',');
            float x = float.Parse(pos[0]);
            float y = float.Parse(pos[1]);
            float z = float.Parse(pos[2].Replace(")", ""));
            GameObject rblock = GameObject.Find(block);
            rblock.transform.position = new Vector3(x, y, z);
            rblock.GetComponent<Voxeme>().targetPosition = rblock.transform.position;
            if (rblock.name.Equals("BlueBlock1"))
            {
                blue1.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("BlueBlock2"))
            {
                blue2.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("RedBlock2"))
            {
                red2.transform.position = rblock.transform.position;
            }
            if (rblock.name.Equals("RedBlock1"))
            {
                red1.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("YellowBlock1"))
            {
                yellow1.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("YellowBlock2"))
            {
                yellow2.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("PinkBlock2"))
            {
                pink2.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("PinkBlock1"))
            {
                pink1.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("GreenBlock1"))
            {
                green1.transform.position = rblock.transform.position;
            }
            if (rblock.name.Equals("GreenBlock2"))
            {
                green2.transform.position = rblock.transform.position;
            }
        }

        if (!splitoutput[2].Contains("speech only"))
        {
            focusCircle1.enabled = true;
            focusCircle1.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find(focus).transform.position);
            //spriteAnimator1.enabled = true;
            //spriteAnimator1.Play("circle_anim_test", 0, 0);
            focusCirc = true;
        }
        if (!splitoutput[2].Contains("pointing only "))
        {
            SetValue("me:speech:intent", splitoutput[0], string.Empty);
        }
    }


    // ----------------------Function: Configurations()---------------------------------
    public string Configurations()
    {

        string config = "";
        Vector3 red1 = GameObject.Find("RedBlock1").transform.position;
        Vector3 red2 = GameObject.Find("RedBlock2").transform.position;
        Vector3 green1 = GameObject.Find("GreenBlock1").transform.position;
        Vector3 green2 = GameObject.Find("GreenBlock2").transform.position;
        Vector3 blue1 = GameObject.Find("BlueBlock1").transform.position;
        Vector3 blue2 = GameObject.Find("BlueBlock2").transform.position;
        Vector3 pink1 = GameObject.Find("PinkBlock1").transform.position;
        Vector3 pink2 = GameObject.Find("PinkBlock2").transform.position;
        Vector3 yellow1 = GameObject.Find("YellowBlock1").transform.position;
        Vector3 yellow2 = GameObject.Find("YellowBlock2").transform.position;
        config = "RedBlock1" + red1.ToString("f9") + " and " + "RedBlock2" + red2.ToString("f9") + " and " + "GreenBlock1" + green1.ToString("f9") + " and "
            + "GreenBlock2" + green2.ToString("f9") + " and " + "BlueBlock1" + blue1.ToString("f9") + " and " + "BlueBlock2" + blue2.ToString("f9") + " and "
            + "PinkBlock1" + pink1.ToString("f9") + " and " + "PinkBlock2" + pink2.ToString("f9") + " and " + "YellowBlock1" + yellow1.ToString("f9") + " and "
            + "YellowBlock2" + yellow2.ToString("f9");


        return config;
    }


    // ----------------------Function: RelationsLog()---------------------------------
    public string RelationsLog()
    {

        string srelation, finalrel;

        List<String> SceneRelations = new List<String>();

        foreach (DictionaryEntry entry in relationTracker.relations)
        {
            string v = (string)entry.Value;
            List<GameObject> keys = (List<GameObject>)entry.Key;
            List<String> relkey = new List<String>();

            foreach (GameObject key1 in (List<GameObject>)entry.Key)
            {
                relkey.Add(key1.name);
            }

            srelation = v + "(" + relkey[0] + ":" + relkey[1] + ")";
            if (srelation.Contains(","))
            {
                SceneRelations.Add(srelation.Replace(",", " & "));
            }
            if (srelation.Contains(":"))
            {
                SceneRelations.Add(srelation.Replace(":", ", "));
            }
            else
            {
                SceneRelations.Add(srelation);
            }
        }
        finalrel = string.Join(" and ", SceneRelations);

        return finalrel;
    }


    public void DianaMREGenerationTest(string config, string output, string focus)
    {

        TargetTimer.Enabled = true;

        string[] splitoutput = output.Split(',');
        //string[] strFocuspos = splitoutput[3].Replace(")", "").Replace("(", "").Split(':');

        //Vector3 Focuspos = new Vector3(float.Parse(strFocuspos[0]), float.Parse(strFocuspos[1]), float.Parse(strFocuspos[2]));
        string newConfig = config.Replace(" and ", ":");
        string[] confi = newConfig.Split(':');


        for (int i = 0; i < confi.Length; i++)
        {

            string[] blockPos = confi[i].Split('(');
            string block = blockPos[0];
            string position = blockPos[1];
            string[] pos = position.Split(',');
            float x = float.Parse(pos[0]);
            float y = float.Parse(pos[1]);
            float z = float.Parse(pos[2].Replace(")", ""));
            GameObject rblock = GameObject.Find(block);
            rblock.transform.position = new Vector3(x, y, z);
            rblock.GetComponent<Voxeme>().targetPosition = rblock.transform.position;

            if (rblock.name.Equals("BlueBlock1"))
            {
                blue1.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("BlueBlock2"))
            {
                blue2.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("RedBlock2"))
            {
                red2.transform.position = rblock.transform.position;
            }
            if (rblock.name.Equals("RedBlock1"))
            {
                red1.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("YellowBlock1"))
            {
                yellow1.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("YellowBlock2"))
            {
                yellow2.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("PinkBlock2"))
            {
                pink2.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("PinkBlock1"))
            {
                pink1.transform.position = rblock.transform.position;

            }
            if (rblock.name.Equals("GreenBlock1"))
            {
                green1.transform.position = rblock.transform.position;
            }
            if (rblock.name.Equals("GreenBlock2"))
            {
                green2.transform.position = rblock.transform.position;
            }
        }

        SetValue("me:intent:targetObj", focus, string.Empty);
        if (!splitoutput[2].Contains("speech only"))
        {
            SetValue("me:intent:lookAt", focus, string.Empty);
            SetValue("me:intent:action", "point", string.Empty);
            SetValue("me:actual:handPosR", GameObject.Find(focus).transform.position, string.Empty);
        }
        if (!splitoutput[2].Contains("pointing only "))
        {
            SetValue("me:speech:intent", splitoutput[0], string.Empty);
        }
        done = true;
    }
}

//Recorded Generated MREs by LLM (TestData)

////1-2
//// Pointing (Human)- Pointing (Diana) 
//string configration1 = "RedBlock1(-0.098242240,1.124870000,0.368312500) and RedBlock2(-0.511479800,1.124870000,0.253864300) and GreenBlock1(0.265574600,1.124870000,-0.446640600) and GreenBlock2(-0.111404000,1.124870000,-0.266982000) and BlueBlock1(0.179088900,1.124870000,0.237997900) and BlueBlock2(0.014448110,1.124870000,-0.159720900) and PinkBlock1(-0.511480500,1.224870000,0.253864600) and PinkBlock2(0.279113600,1.125561000,0.237998100) and YellowBlock1(-0.315363000,1.124870000,-0.126586800) and YellowBlock2(0.049953640,1.124870000,0.409480800)";
//string focusObj1 = "PinkBlock2";
//string output1 = "No speech , None , pointing only , (0.279113600: 1.125561000: 0.237998100)";
//string output_h1 = "No speech , None , pointing only , (0.279113600: 1.125561000: 0.237998100)";

////3-4
//// Multimodal Attributive (Human)- Pointing (Diana)
//string configration2 = "GreenBlock1(-0.095285360,1.124870000,-0.381064100) and GreenBlock2(-0.185411600,1.124870000,0.288290400) and BlueBlock1(0.460459500,1.124870000,0.115663700) and BlueBlock2(-0.232044700,1.124870000,0.414967500) and PinkBlock1(0.032032460,1.124870000,0.179788100) and PinkBlock2(-0.487780700,1.124870000,0.414433800) and YellowBlock1(-0.095537300,1.325110000,-0.380485800) and YellowBlock2(0.070100950,1.124870000,-0.286942700)";
//string focusObj2 = "RedBlock2";
//string output2 = "No speech , None , pointing only , (0.100276400: 1.224870000: 0.286603300)";
//string output_h2 = "This block , this , multimodal , (0.100276400: 1.224870000: 0.286603300)";

////5-6
////Speech only Relational (Human)- Speech Only Relational (Diana)
//string configration3 = "RedBlock1(-0.052341540,1.124870000,-0.395484600) and RedBlock2(-0.298598900,1.124870000,0.345127900) and GreenBlock1(0.115968600,1.124870000,-0.082615180) and GreenBlock2(0.476863700,1.124870000,0.012207040) and BlueBlock1(0.368054300,1.124870000,-0.468603100) and BlueBlock2(0.014476060,1.124870000,0.198259100) and PinkBlock1(-0.395499800,1.124870000,-0.303294900) and PinkBlock2(0.350860100,1.124870000,0.168934200) and YellowBlock1(0.350864300,1.124870000,0.268937700) and YellowBlock2(0.452714500,1.124870000,-0.304579600) ";
//string focusObj3 = "BlueBlock2";
//string output3 = "The blue block to the right of the plate , the , speech only , (0.768054300: 1.124870000: -0.468603100)";
//string output_h3 = "The blue block next to the green block, None , speech only ,  (0.768054300: 1.124870000: -0.468603100)";

////7-8
////Pointing (Human)- Multimodal Attributive RE (Diana)
//string configration4 = "RedBlock1(-0.335300100,1.124870000,-0.432426100) and RedBlock2(0.433030800,1.124870000,0.248496100) and GreenBlock1(-0.268699600,1.124870000,-0.313648500) and GreenBlock2(0.085234370,1.124870000,-0.119364100) and BlueBlock1(0.395773700,1.124870000,-0.316980500) and BlueBlock2(-0.511329100,1.124870000,0.345023700) and PinkBlock1(-0.500331900,1.124870000,0.058788850) and PinkBlock2(0.395807400,1.124893000,-0.216973700) and YellowBlock1(0.078200360,1.125253000,0.223089800) and YellowBlock2(0.395815800,1.224871000,-0.216962100)";
//string focusObj4 = "PinkBlock1";
//string output4 = "This pink block , the , multimodal , (-0.500331900: 1.124870000: 0.058788850)";
//string output_h4 = "No speech , None , pointing only , (-0.500331900: 1.124870000: 0.058788850)";

////9-10
//// Multimodal Relational (Human) - Multimodal Relational (Diana)
//string configration5 = "RedBlock1(0.515165300,1.124870000,0.324439400) and RedBlock2(-0.471917100,1.124870000,-0.229951700) and GreenBlock1(-0.254069800,1.124870000,0.411431500) and GreenBlock2(-0.080378230,1.124870000,-0.350180400) and BlueBlock1(-0.254070400,1.224870000,0.411431600) and BlueBlock2(-0.157128400,1.124870000,-0.104218900) and PinkBlock1(0.069581330,1.724870000,0.361659100) and PinkBlock2(-0.310167400,1.124870000,-0.399751900) and YellowBlock1(-0.030419330,1.124870000,0.361659300) and YellowBlock2(-0.412302200,1.124870000,-0.013553920)";
//string focusObj5 = "PinkBlock2";
//string output5 = "The pink block to the right of the blue block , the , multimodal , (-0.310167400:1.124870000:-0.399751900)";
//string output_h5 = "Grasp the pink block behind the yellow block , the , multimodal , (-0.310167400:1.124870000:-0.399751900)";

////11-12
////Speech only Relational (Human) - Multimodal attributive (Diana)
//string configration6 = "RedBlock1(-0.413386700,1.124870000,-0.336820500) and RedBlock2(-0.198345900,1.124870000,0.381917500) and GreenBlock1(-0.470635700,1.124870000,0.376472300) and GreenBlock2(0.261373700,1.124870000,-0.214361700) and BlueBlock1(0.472036900,1.124614000,0.026368190) and BlueBlock2(0.010544860,1.124870000,0.171638300) and PinkBlock1(-0.476316600,1.124870000,0.245340300) and PinkBlock2(0.118039400,1.124634000,0.026376140) and YellowBlock1(0.378827200,1.124870000,0.344435200) and YellowBlock2(-0.089455160,1.124870000,0.171638200) ";
//string focusObj6 = "YellowBlock1";
//string output6 = "The yellow block , the , multimodal , (0.378827200: 1.124870000: 0.344435200)";
//string output_h6 = "Lift the yellow block behind the plate , the , speech only , (0.378827200: 1.124870000: 0.344435200)";

////13-14
////Speech only Attributive (Human) - Multimodal Attributive (Diana)
//string configration7 = "RedBlock1(0.003966093,1.124870000,0.244562800) and RedBlock2(0.476666400,1.124870000,-0.103516800) and GreenBlock1(0.096785370,1.124870000,-0.281754500) and GreenBlock2(0.576667500,1.124870000,-0.103508700) and BlueBlock1(-0.345503800,1.124861000,0.026342260) and BlueBlock2(0.226002600,1.124870000,-0.220365600) and PinkBlock1(-0.123803800,1.124858000,0.026350290) and PinkBlock2(0.384387800,1.124870000,-0.457408200) and YellowBlock1(-0.007722884,1.124870000,-0.377586800) and YellowBlock2(0.550246500,1.124870000,0.271873800) ";
//string focusObj7 = "GreenBlock2";
//string output7 = "The green block , the , multimodal , (0.576667500: 1.124870000: -0.103508700)";
//string output_h7 = "The green block , the , speech only , (0.576667500: 1.124870000: -0.103508700)";

////15-16
////Speech only Attributive (Human)- Speech only Relational (Diana)
//string configration8 = "RedBlock2(0.094296800,1.124870000,0.414771600) and GreenBlock1(-0.338806600,1.124870000,0.139364700) and GreenBlock2(-0.148490400,1.124870000,-0.306954400) and BlueBlock1(-0.466068200,1.124870000,0.396407900) and BlueBlock2(0.119693600,1.124870000,-0.325370000) and PinkBlock1(0.421158400,1.124870000,0.312187000) and PinkBlock2(0.502657200,1.124870000,-0.370804400) and YellowBlock1(0.069874850,1.124870000,-0.444167700) and YellowBlock2(-0.286639000,1.124870000,-0.285298900)";
//string focusObj8 = "GreenBlock1";
//string output8 = "The green block to the right of the cup , the , speech only , (-0.338806600:1.124870000:0.139364700)";
//string output_h8= "Lift the green block , the , speech only , (-0.338806600:1.124870000:0.139364700)";

////17-18
////Multimodal Attributive (Human)-Multimodal Attributive (Diana)
//string configration9 = "RedBlock1(0.206279800,1.124870000,0.353480400) and RedBlock2(0.203628700,1.124848000,-0.219047600) and GreenBlock1(0.103595900,1.124870000,-0.219032100) and GreenBlock2(-0.189662700,1.124870000,0.414821200) and BlueBlock1(0.047915340,1.124870000,0.354370900) and BlueBlock2(-0.388291500,1.124870000,-0.079547290) and PinkBlock1(-0.503770900,1.124870000,0.245382600) and PinkBlock2(0.535650000,1.124870000,-0.236901400) and YellowBlock1(0.259186300,1.124870000,0.217477100) and YellowBlock2(0.012891830,1.124870000,0.146721800)";
//string focusObj9 = "RedBlock1";
//string output9 = "The red block , the , multimodal , (0.206279800: 1.124870000: 0.353480400)";
//string output_h9 = "Pick up the red block , the , multimodal , (0.206279800: 1.124870000: 0.353480400)";

// new output1 diana - output2 human
//Human-diana
//19-20 pointing - multi
//string configration1 = "RedBlock1(-0.304282900,1.124870000,0.312935300) and RedBlock2(0.471954100,1.124870000,0.037330110) and GreenBlock1(0.555349800,1.124870000,-0.200412900) and GreenBlock2(0.114975200,1.124870000,0.201137500) and BlueBlock1(-0.172570600,1.124870000,-0.183206700) and BlueBlock2(0.254922200,1.124870000,-0.155047500) and PinkBlock1(0.289217900,1.124870000,0.356389900) and PinkBlock2(-0.345538900,1.124963000,0.026299100) and YellowBlock1(-0.404282600,1.124870000,0.312935300) and YellowBlock2(0.214975400,1.124870000,0.201137500)";
//string focusObj1 = "PinkBlock1";
//string output1 = "put it next to the cup , None , multimodal , (0.289217900: 1.124870000: 0.356389900)";
//string output_h1 = "No speech , None , pointing only , (0.289217900: 1.124870000: 0.356389900)";

////21-22 multi -multi
//string configration2 = "RedBlock1(-0.327527300,1.124870000,-0.358055500) and RedBlock2(0.207265300,1.124870000,0.238088600) and GreenBlock1(-0.057521310,1.124870000,-0.071387830) and GreenBlock2(-0.170663900,1.124870000,-0.097378220) and BlueBlock1(-0.447411400,1.124870000,0.414257900) and BlueBlock2(0.264754400,1.124870000,-0.272544400) and PinkBlock1(0.448824200,1.124870000,0.251994900) and PinkBlock2(-0.394722600,1.124870000,0.203768600) and YellowBlock1(0.448824300,1.124870000,0.151995000) and YellowBlock2(0.137101600,1.124870000,-0.164803000) ";
//string focusObj2 = "GreenBlock2";
//string output2 = "green block , the , multimodal , (-0.170663900: 1.124870000: -0.097378220)";
//string output_h2 = "pick up the green block next to the yellow block , the , multimodal , (-0.170663900: 1.124870000: -0.097378220)";

////23-24 multi -multi
//string configration3 = "RedBlock1(0.239870300,1.124875000,-0.286779700) and RedBlock2(-0.176487200,1.124870000,-0.318877400) and GreenBlock1(0.207450900,1.124887000,0.320743700) and GreenBlock2(0.456449600,1.125270000,0.270388600) and BlueBlock1(-0.114038400,1.124869000,0.227485300) and BlueBlock2(0.339990600,1.224931000,-0.286764600) and PinkBlock1(0.339905500,1.124870000,-0.286802800) and PinkBlock2(-0.099835990,1.124870000,0.034944530) and YellowBlock1(-0.214051800,1.124908000,0.327530200) and YellowBlock2(-0.697487600,1.124871000,-0.128596900) ";
//string focusObj3 = "YellowBlock1";
//string output3 = "pick up the yellow block , the , multimodal , (-0.214051800: 1.124908000: 0.327530200)";
//string output_h3 = "grab the yellow block , the , multimodal , (-0.214051800: 1.124908000: 0.327530200)";

////25-26 multi -multi
//string configration4 = "RedBlock1(0.525303500,1.124870000,0.250442300) and RedBlock2(0.003428608,1.124870000,0.249775400) and GreenBlock1(0.045049940,1.124870000,-0.281490000) and GreenBlock2(-0.046334890,1.124870000,-0.124036200) and BlueBlock1(-0.573868500,1.124870000,0.396311000) and BlueBlock2(-0.471547700,1.124870000,-0.245447000) and PinkBlock1(-0.180346500,1.124870000,-0.461008800) and PinkBlock2(-0.573868900,1.124870000,0.296311300) and YellowBlock1(-0.313536000,1.124870000,-0.197670300) and YellowBlock2(-0.241498400,1.124870000,0.172357800)";
//string focusObj4 = "PinkBlock2";
//string output4 = "the pink block next to the blue block , the , multimodal , (-0.573868900: 1.124870000: 0.296311300)";
//string output_h4 = "the pink block , the , multimodal , (-0.573868900: 1.124870000: 0.296311300)";

////27-28  multi -multi
//string configration5 = "RedBlock1(-0.139815000,1.124870000,0.372531200) and RedBlock2(-0.016933990,1.124870000,0.111737100) and GreenBlock1(0.119304200,1.124871000,-0.106413600) and GreenBlock2(-0.474992900,1.124870000,-0.083098980) and BlueBlock1(0.421908400,1.124870000,0.286555800) and BlueBlock2(-0.589277000,1.124872000,0.249377000) and PinkBlock1(-0.135676400,1.124870000,-0.221934300) and PinkBlock2(-0.443727400,1.124870000,-0.364785300) and YellowBlock1(-0.193644600,1.124870000,-0.450280200) and YellowBlock2(0.221892000,1.124869000,0.286554900)";
//string focusObj5 = "GreenBlock2";
//string output5 = "green block , the , multimodal , (-0.170663900: 1.124870000: -0.097378220)";
//string output_h5 = "pick up the green block next to the yellow block , the , multimodal , (-0.170663900: 1.124870000: -0.097378220)";

////29-30  multi -multi
//string configration6 = "RedBlock1(0.176816700,1.124870000,-0.264214200) and RedBlock2(-0.299315800,1.124870000,-0.290655700) and GreenBlock1(-0.185956700,1.124870000,0.362809500) and GreenBlock2(-0.310272900,1.124870000,-0.449459100) and BlueBlock1(0.313130400,1.124870000,-0.301424200) and BlueBlock2(0.432481300,1.124870000,-0.397751800) and PinkBlock1(-0.513236100,1.124870000,0.130409000) and PinkBlock2(0.176816900,1.224871000,-0.264214200) and YellowBlock1(0.431231000,1.124870000,-0.212444400) and YellowBlock2(0.527030200,1.124870000,0.359422200) ";
//string focusObj6 = "PinkBlock2";
//string output6 = "the pink block , the , multimodal , (0.176816700: 1.124871000: -0.264214200)";
//string output_h6 = "pick upwards the pink block , the , multimodal , (0.176816900: 1.224871000: -0.264214200)";

////31-32  multi -speech
//string configration7 = "RedBlock1(-0.296343700,1.124870000,-0.130904700) and RedBlock2(0.101794000,1.124870000,0.196326500) and GreenBlock1(-0.504379000,1.124870000,-0.239979000) and GreenBlock2(0.519552900,1.124870000,-0.280025800) and BlueBlock1(0.519553400,1.224870000,-0.280026300) and BlueBlock2(0.295002000,1.124764000,-0.150611900) and PinkBlock1(-0.485233600,1.124870000,-0.023482280) and PinkBlock2(-0.267630500,1.124870000,-0.249912300) and YellowBlock1(0.378154600,1.124870000,0.410348300) and YellowBlock2(-0.416864600,1.124870000,-0.397103800)";
//string focusObj7 = "RedBlock2";
//string output7 = "the red block , the , multimodal , (0.101794000: 1.124870000: 0.196326500)";
//string output_h7 = "Take the red block , the , speech only , (0.101794000: 1.124870000: 0.196326500)";

////33-34  multi -multi
//string configration8 = "RedBlock1(0.199053300,1.124870000,-0.243319400) and RedBlock2(-0.064961820,1.124870000,0.396758700) and GreenBlock1(0.550468200,1.124870000,-0.430612500) and GreenBlock2(-0.083664100,1.124870000,-0.083408740) and BlueBlock1(-0.319582400,1.124870000,-0.446711700) and BlueBlock2(0.511201500,1.124870000,0.330992300) and PinkBlock1(-0.002046227,1.124870000,-0.310666900) and PinkBlock2(-0.203375500,1.124870000,0.190286900) and YellowBlock1(-0.491916500,1.224870000,0.321276000) and YellowBlock2(-0.491916100,1.124870000,0.321275600)";
//string focusObj8 = "PinkBlock2";
//string output8 = "the pink block , the , multimodal , (-0.203375500: 1.124870000: 0.190286900)";
//string output_h8 = "grab the pink block , the , multimodal , (-0.203375500: 1.124870000: 0.190286900)";

////35-36 multi -multi
//string configration9 = "RedBlock1(0.108630600,1.124870000,0.350298100) and RedBlock2(-0.214082700,1.124870000,-0.454856700) and GreenBlock1(0.512988000,1.124870000,0.332843100) and GreenBlock2(-0.073187190,1.124870000,-0.193133300) and BlueBlock1(-0.468428800,1.124870000,-0.213389300) and BlueBlock2(-0.139257400,1.124870000,0.208151100) and PinkBlock1(-0.073183470,1.224870000,-0.193106500) and PinkBlock2(-0.389313000,1.124870000,0.149436600) and YellowBlock1(-0.489830500,1.124870000,-0.401649000) and YellowBlock2(0.538037700,1.124870000,-0.311077000)";
//string focusObj9 = "BlueBlock1";
//string output9 = "the blue block , the , multimodal , (-0.468428800: 1.124870000: -0.213389300)";
//string output_h9 = "lift the blue block , the , multimodal , (-0.468428800: 1.124870000: -0.213389300)";

////37-38 multi -multi
//string configration10 = "RedBlock1(0.592290900,1.124870000,-0.295081400) and RedBlock2(0.008241653,1.124870000,0.189849100) and GreenBlock1(0.538551900,1.124870000,-0.194459100) and GreenBlock2(-0.505525300,1.224879000,-0.179950000) and BlueBlock1(0.374972800,1.124870000,0.221425900) and BlueBlock2(-0.322637700,1.124870000,0.172357800) and PinkBlock1(-0.505525000,1.124870000,-0.179950100) and PinkBlock2(0.186588200,1.124870000,-0.348105200) and YellowBlock1(-0.032398640,1.124879000,-0.344306600) and YellowBlock2(0.553785000,1.124870000,0.349984900)";
//string focusObj10 = "YellowBlock1";
//string output10 = "pick up the yellow block , the , multimodal , (-0.032398640: 1.124879000: -0.344306600)";
//string output_h10 = "this block , this , multimodal , (-0.032398640: 1.124879000: -0.344306600)";

////39-40 multi -speech
//string configration11 = "RedBlock1(0.294995500,1.224871000,0.203377800) and RedBlock2(-0.025642990,1.124870000,-0.242599400) and GreenBlock1(0.481602700,1.124870000,-0.194570200) and GreenBlock2(0.294994000,1.124640000,0.203378300) and BlueBlock1(0.233285100,1.124870000,-0.208186800) and BlueBlock2(0.020286260,1.124870000,-0.377099900) and PinkBlock1(0.133964100,1.124870000,0.210130900) and PinkBlock2(0.133964000,1.124870000,0.310131300) and YellowBlock1(-0.109503600,1.124871000,0.324016900) and YellowBlock2(-0.345483200,1.124850000,0.026299100)";
//string focusObj11 = "BlueBlock1";
//string output11 = "the blue block to the right of the green block , the , multimodal , (0.233285100: 1.124870000: -0.208186800)";
//string output_h11 = "lift the blue block in front of the plate , the , speech only , (0.233285100: 1.124870000: -0.208186800)";

////41-42 multi -multi
//string configration12 = "RedBlock1(0.531414800,1.124870000,0.167591300) and RedBlock2(-0.154270400,1.124870000,-0.443729800) and GreenBlock1(-0.441105700,1.124870000,-0.307256700) and GreenBlock2(-0.595351700,1.124871000,-0.317143900) and BlueBlock1(0.328511300,1.124870000,0.397166400) and BlueBlock2(0.568750300,1.124870000,0.398294000) and PinkBlock1(-0.374911800,1.124870000,0.089465410) and PinkBlock2(-0.054269400,1.124871000,-0.443727400) and YellowBlock1(-0.238828700,1.124870000,0.268930600) and YellowBlock2(-0.128353300,1.124870000,0.172511000)";
//string focusObj12 = "RedBlock2";
//string output12 = "the red block , the , multimodal , (-0.154270400: 1.124870000: -0.443729800)";
//string output_h12 = "pick the red block , the , multimodal , (-0.154270400: 1.124870000: -0.443729800)";

////43-44 multi -speech
//string configration13 = "RedBlock1(-0.015303730,1.124870000,0.002760455) and RedBlock2(0.326293200,1.124870000,-0.437550000) and GreenBlock1(0.440192100,1.124870000,0.284377100) and GreenBlock2(0.182971300,1.124870000,-0.357689700) and BlueBlock1(0.068043600,1.124870000,0.272987200) and BlueBlock2(-0.510570300,1.124870000,-0.289925000) and PinkBlock1(0.490037500,1.124870000,-0.371380000) and PinkBlock2(-0.456131800,1.124870000,0.361764900) and YellowBlock1(-0.249550700,1.124870000,-0.184457400) and YellowBlock2(0.355318800,1.124870000,0.404352600)";
//string focusObj13 = "BlueBlock2";
//string output13 = "the blue block to the right of the plate , the , speech only , (-0.510570300: 1.124870000: -0.289925000)";
//string output_h13 = "This blue cube , None , multimodal , (-0.510570300: 1.124870000: -0.289925000)";

////45-46 multi -multi
//string configration14 = "RedBlock1(-0.097416660,1.125127000,-0.152294600) and RedBlock2(-0.110404200,1.124919000,0.275637400) and GreenBlock1(-0.072762650,1.149221000,0.064455520) and GreenBlock2(-0.319034400,1.124870000,0.202901100) and BlueBlock1(-0.302781800,1.124870000,-0.414393800) and BlueBlock2(-0.490903100,1.124870000,0.138156100) and PinkBlock1(0.326105500,1.124870000,-0.408620400) and PinkBlock2(0.375118900,1.124870000,0.219119500) and YellowBlock1(-0.737141800,1.124870000,-0.380026500) and YellowBlock2(-0.737140800,1.124870000,-0.480026300)";
//string focusObj14 = "BlueBlock2";
//string output14 = "the blue block , the , multimodal , (-0.490903100: 1.124870000: 0.138156100) , the , speech only , (-0.510570300: 1.124870000: -0.289925000)";
//string output_h14 = "the blue block to the left of the green block , the , multimodal , (-0.490903100: 1.124870000: 0.138156100)";

////47-48 multi -multi
//string configration15 = "RedBlock1(0.343816300,1.124870000,-0.239587300) and RedBlock2(0.493243700,1.124870000,-0.349320500) and GreenBlock1(-0.258438600,1.124872000,0.159764800) and GreenBlock2(-0.158325800,1.124867000,0.159944000) and BlueBlock1(-0.082249580,1.124891000,-0.274599600) and BlueBlock2(0.139693300,1.124870000,-0.250548200) and PinkBlock1(-0.714146700,1.124871000,-0.227725600) and PinkBlock2(-0.509818600,1.124869000,-0.195831100) and YellowBlock1(0.491097300,1.124870000,-0.056270840) and YellowBlock2(0.534856600,1.124870000,0.170227900)";
//string focusObj15 = "BlueBlock2";
//string output15 = "this blue block , the , multimodal , (0.082249580: 1.124891000: -0.274599600)";
//string output_h15 = "hold the blue block to the right of the red block , the , multimodal , (-0.082249580: 1.124891000: -0.274599600)";

////49-50 multi -multi
//string configration16 = "RedBlock1(-0.463808200,1.124870000,-0.401764800) and RedBlock2(-0.303210400,1.124870000,-0.295266700) and GreenBlock1(0.347380800,1.224873000,-0.291779000) and GreenBlock2(0.053202420,1.124870000,-0.357390600) and BlueBlock1(0.326961100,1.124870000,-0.428812200) and BlueBlock2(0.045364620,1.124870000,0.186738100) and PinkBlock1(0.307182900,1.124870000,0.214270200) and PinkBlock2(-0.494659000,1.124870000,-0.265827600) and YellowBlock1(0.347381500,1.124870000,-0.291778000) and YellowBlock2(-0.204411800,1.124870000,-0.464221400)";
//string focusObj16 = "GreenBlock1";
//string output16 = "pick this green block , the , multimodal , (0.347380800: 1.224873000: -0.291779000)";
//string output_h16 = "green block on top of yellow block , the , multimodal , (0.347380800: 1.224873000: -0.291779000)";

////51-52 multi -multi
//string configration17 = "RedBlock1(-0.327527300,1.124870000,-0.358055500) and RedBlock2(0.207265300,1.124870000,0.238088600) and GreenBlock1(-0.057521310,1.124870000,-0.071387830) and GreenBlock2(-0.242133400,1.124870000,0.299286400) and BlueBlock1(-0.447411400,1.124870000,0.414257900) and BlueBlock2(-0.087663350,1.124870000,0.386163800) and PinkBlock1(0.448824200,1.124870000,0.251994900) and PinkBlock2(-0.394722600,1.124870000,0.203768600) and YellowBlock1(0.448824300,1.124870000,0.151995000) and YellowBlock2(0.495020200,1.125279000,-0.445981600) ";
//string focusObj17 = "RedBlock1";
//string output17 = "the red block , the , multimodal , (0.327527300:1.124870000:0.358055500)";
//string output_h17 = "grab the red block , the , multimodal , (-0.327527300: 1.124870000: -0.358055500)";

////53-54 multi -multi
//string configration18 = "RedBlock1(-0.150941900,1.124870000,-0.422809700) and RedBlock2(0.579122400,1.124870000,-0.276693500) and GreenBlock1(0.382615500,1.124870000,0.253874700) and GreenBlock2(0.025034430,1.124870000,0.195167500) and BlueBlock1(0.102904900,1.124870000,-0.131045400) and BlueBlock2(0.558197400,1.124870000,0.125362400) and PinkBlock1(0.471984700,1.124774000,0.026372600) and PinkBlock2(-0.413957700,1.124870000,-0.369752000) and YellowBlock1(0.209692300,1.124870000,-0.159628000) and YellowBlock2(-0.132176500,1.124870000,0.185493900)";
//string focusObj18 = "PinkBlock1";
//string output18 = "pick up the pink block next to the plate , the , multimodal , (0.471984700: 1.124774000: 0.026372600)";
//string output_h18 = "the pink block next to the plate , the , multimodal , (0.471984700: 1.124774000: 0.026372600)";

////55-56 multi -multi
//string configration19 = "RedBlock1(0.072875950,1.124870000,0.143507100) and RedBlock2(-0.328490600,1.124870000,0.317129800) and GreenBlock1(0.437496500,1.124870000,-0.242018800) and GreenBlock2(-0.119948900,1.124870000,-0.153451300) and BlueBlock1(-0.514057500,1.124870000,-0.035204830) and BlueBlock2(-0.144704800,1.124870000,-0.359856700) and PinkBlock1(0.088442370,1.124870000,0.337150900) and PinkBlock2(0.506788400,1.124870000,0.135217300) and YellowBlock1(0.301122500,1.124870000,-0.409010400) and YellowBlock2(-0.306641800,1.124870000,-0.189563300) ";
//string focusObj19 = "YellowBlock2";
//string output19 = "pick up the yellow block , the , multimodal , (-0.306641800: 1.124870000: -0.189563300)";
//string output_h19 = "the yellow block to the green block , the , multimodal , (-0.306641800: 1.124870000: -0.189563300)";

////57-58 multi -multi
//string configration20 = "RedBlock1(-0.097416660,1.125127000,-0.152294600) and RedBlock2(0.326105700,1.124870000,-0.308490700) and GreenBlock1(-0.072762650,1.149221000,0.064455520) and GreenBlock2(-0.319034400,1.124870000,0.202901100) and BlueBlock1(-0.302781800,1.124870000,-0.414393800) and BlueBlock2(-0.490903100,1.124870000,0.138156100) and PinkBlock1(0.326105500,1.124870000,-0.408620400) and PinkBlock2(0.375118900,1.124870000,0.219119500) and YellowBlock1(-0.737141800,1.124870000,-0.380026500) and YellowBlock2(-0.737140800,1.124870000,-0.480026300)";
//string focusObj20 = "BlueBlock2";
//string output20 = "the blue block , the , multimodal , (0.490903100: 1.124870000: 0.138156100)";
//string output_h20 = "blue block , the , multimodal , (-0.490903100: 1.124870000: 0.138156100)";

////59-60 multi -speech
//string configration21 = "RedBlock1(0.136510600,1.124870000,-0.294299100) and RedBlock2(0.303653900,1.250528000,0.148112800) and GreenBlock1(-0.705238600,1.124870000,0.049294940) and GreenBlock2(0.137034900,1.126884000,0.275573000) and BlueBlock1(-0.213484500,1.124870000,0.400106300) and BlueBlock2(0.136511100,1.224870000,-0.294299100) and PinkBlock1(-0.582689700,1.151994000,-0.371356200) and PinkBlock2(-0.507939900,1.125242000,-0.185659700) and YellowBlock1(-0.705240200,1.224870000,0.049295010) and YellowBlock2(0.364784700,1.149145000,0.101874200)";
//string focusObj21 = "BlueBlock1";
//string output21 = "Pick the blue block and put it on a pink block , None , multimodal , (-0.213484500: 1.124870000: 0.400106300)";
//string output_h21 = "move the blue block to the peak of the pink block , the , speech only ,  (-0.213484500: 1.124870000: 0.400106300)";

////61-62 multi -multi
//string configration22 = "RedBlock1(-0.095285650,1.224870000,-0.381064200) and RedBlock2(0.100276400,1.224870000,0.286603300) and GreenBlock1(-0.095285360,1.124870000,-0.381064100) and GreenBlock2(-0.185411600,1.124870000,0.288290400) and BlueBlock1(0.460459500,1.124870000,0.115663700) and BlueBlock2(-0.232044700,1.124870000,0.414967500) and PinkBlock1(0.032032460,1.124870000,0.179788100) and PinkBlock2(-0.487780700,1.124870000,0.414433800) and YellowBlock1(-0.095537300,1.325110000,-0.380485800) and YellowBlock2(0.070100950,1.124870000,-0.286942700";
//string focusObj22 = "BlueBlock1";
//string output22 = "the blue block , the , multimodal , (0.460459500: 1.124870000: 0.115663700)";
//string output_h22 = "this block , this , multimodal , (0.460459500: 1.124870000: 0.115663700)";

////63-64 multi -multi
//string configration23 = "RedBlock1(0.515165300,1.124870000,0.324439400) and RedBlock2(-0.471917100,1.124870000,-0.229951700) and GreenBlock1(-0.254069800,1.124870000,0.411431500) and GreenBlock2(-0.080378230,1.124870000,-0.350180400) and BlueBlock1(0.330922600,1.124870000,0.353068600) and BlueBlock2(0.306601400,1.124870000,-0.172610500) and PinkBlock1(0.069581330,1.124870000,0.361659100) and PinkBlock2(-0.310167400,1.124870000,-0.399751900) and YellowBlock1(-0.057126270,1.124870000,-0.104219000) and YellowBlock2(-0.412302200,1.124870000,-0.013553920) ";
//string focusObj23 = "BlueBlock2";
//string output23 = "the blue block in front of the plate , the , multimodal , (0.306601400: 1.124870000: -0.172610500)";
//string output_h23 = "select the blue block , the , multimodal , (0.306601400: 1.124870000: -0.172610500)";

////65-66 multi -multi
//string configration24 = "RedBlock1(0.108630600,1.124870000,0.350298100) and RedBlock2(-0.214082700,1.124870000,-0.454856700) and GreenBlock1(0.512988000,1.124870000,0.332843100) and GreenBlock2(-0.073187190,1.124870000,-0.193133300) and BlueBlock1(-0.468428800,1.124870000,-0.213389300) and BlueBlock2(-0.139257400,1.124870000,0.208151100) and PinkBlock1(-0.201771900,1.124870000,-0.284343000) and PinkBlock2(-0.389313000,1.124870000,0.149436600) and YellowBlock1(-0.489830500,1.124870000,-0.401649000) and YellowBlock2(0.569138300,1.124870000,-0.213633200)";
//string focusObj24 = "YellowBlock2";
//string output24 = "pick this up , None , multimodal , (0.569138300: 1.124870000: -0.213633200)";
//string output_h24 = "the yellow block , the , multimodal , (0.569138300: 1.124870000: -0.213633200)";

////67-68 multi -multi
//string configration25 = "RedBlock1(-0.528061900,1.124870000,-0.012271990) and RedBlock2(0.471981100,1.124639000,0.026385200) and GreenBlock1(-0.716731500,1.124870000,-0.037039050) and GreenBlock2(-0.411182100,1.124870000,-0.442338100) and BlueBlock1(0.472014800,1.224911000,0.026379320) and BlueBlock2(0.472907200,1.325106000,0.026124060) and PinkBlock1(-0.146619000,1.224878000,-0.257757300) and PinkBlock2(0.475304400,1.423488000,0.024907910) and YellowBlock1(-0.411182200,1.224870000,-0.442338400) and YellowBlock2(-0.146619000,1.124870000,-0.257757400)";
//string focusObj25 = "YellowBlock1";
//string output25 = "the yellow block , the , multimodal , (-0.411182200: 1.224870000: -0.442338400)";
//string output_h25 = "pick this yellow block , the , multimodal , (-0.411182200: 1.224870000: -0.442338400)";

////69-70 multi -speech
//string configration26 = "RedBlock2(-0.109168000,1.124870000,0.180804700) and GreenBlock1(0.494292700,1.124870000,0.176121200) and GreenBlock2(-0.473071200,1.124870000,0.239771200) and BlueBlock1(-0.297692500,1.124870000,-0.455099400) and BlueBlock2(0.201625100,1.124870000,-0.445249500) and PinkBlock1(-0.108625600,1.124870000,-0.135128600) and PinkBlock2(0.268388600,1.124870000,-0.151941800) and YellowBlock1(0.004848123,1.124870000,-0.237282500) and YellowBlock2(-0.475653600,1.124870000,-0.370448100)";
//string focusObj26 = "PinkBlock1";
//string output26 = "the pink block , the , multimodal , (-0.108625600: 1.124870000: -0.135128600)";
//string output_h26 = "the pink block next to the plate , the , speech only , (-0.108625600: 1.124870000: -0.135128600)";

////71-72 multi -multi
//string configration27 = "RedBlock1(-0.097416660,1.125127000,-0.152294600) and RedBlock2(-0.110404200,1.124919000,0.275637400) and GreenBlock1(-0.072762650,1.149221000,0.064455520) and GreenBlock2(-0.319034400,1.124870000,0.202901100) and BlueBlock1(-0.302781800,1.124870000,-0.414393800) and BlueBlock2(-0.490903100,1.124870000,0.138156100) and PinkBlock1(0.326105500,1.124870000,-0.408620400) and PinkBlock2(0.375118900,1.124870000,0.219119500) and YellowBlock1(-0.737141800,1.124870000,-0.380026500) and YellowBlock2(-0.737140800,1.124870000,-0.480026300) ";
//string focusObj27 = "BlueBlock2";
//string output27 = "the blue block , the , multimodal , (0.490903100: 1.124870000: 0.138156100)";
//string output_h27 = "the blue block to the left of the green block , the , multimodal , (-0.490903100: 1.124870000: 0.138156100)";

////73-74 multi -multi
//string configration28 = "RedBlock1(0.296634000,1.149235000,-0.095323840) and RedBlock2(0.207265300,1.124870000,0.238088600) and GreenBlock1(-0.057521310,1.124870000,-0.071387830) and GreenBlock2(-0.242133400,1.124870000,0.299286400) and BlueBlock1(-0.447411400,1.124870000,0.414257900) and BlueBlock2(-0.087663350,1.124870000,0.386163800) and PinkBlock1(0.448824200,1.124870000,0.251994900) and PinkBlock2(-0.394722600,1.124870000,0.203768600) and YellowBlock1(0.448824300,1.124870000,0.151995000) and YellowBlock2(0.495020200,1.125279000,-0.445981600)";
//string focusObj28 = "BlueBlock2";
//string output28 = "Take this blue block , the , multimodal , (0.087663350:1.124870000:0.386163800)";
//string output_h28 = "grab the blue block , the , multimodal , (-0.087663350: 1.124870000: 0.386163800)";

////75-76 multi -multi
//string configration29 = "RedBlock1(0.239870300,1.124875000,-0.286779700) and RedBlock2(-0.176487200,1.124870000,-0.318877400) and GreenBlock1(0.207450900,1.124887000,0.320743700) and GreenBlock2(0.207450900,1.224870000,0.320737900) and BlueBlock1(-0.114038400,1.124869000,0.227485300) and BlueBlock2(0.339990600,1.224931000,-0.286764600) and PinkBlock1(0.339905500,1.124870000,-0.286802800) and PinkBlock2(-0.099835990,1.124870000,0.034944530) and YellowBlock1(-0.214051800,1.124908000,0.327530200) and YellowBlock2(-0.697487600,1.124871000,-0.128596900)";
//string focusObj29 = "GreenBlock2";
//string output29 = "This green block , the , multimodal , (0.207450900: 1.224870000: 0.320737900)";
//string output_h29 = "grab the green block , the , multimodal , (0.207450900: 1.224870000: 0.320737900)";

////77-78 multi -multi
//string configration30 = "RedBlock1(0.132892500,1.124870000,0.253722600) and RedBlock2(0.486959200,1.124870000,0.233281000) and GreenBlock1(-0.503483200,1.124870000,-0.367357600) and GreenBlock2(0.354008000,1.124870000,-0.419901900) and BlueBlock1(-0.218613600,1.124870000,0.371223200) and BlueBlock2(-0.047166970,1.124870000,-0.165373200) and PinkBlock1(0.486959500,1.224870000,0.233281000) and PinkBlock2(-0.117981000,1.124870000,0.205420200) and YellowBlock1(0.177631200,1.124870000,-0.292458100) and YellowBlock2(0.333840800,1.124870000,0.284314200)";
//string focusObj30 = "GreenBlock2";
//string output30 = "pick up the green block , the , multimodal , (0.354008000: 1.124870000: -0.419901900)";
//string output_h30 = "green block , the , multimodal , (0.354008000: 1.124870000: -0.419901900)";

////79-80 speech-speech
//string configration31 = "RedBlock1(-0.330902900,1.124870000,0.293514900) and RedBlock2(-0.492411100,1.124870000,0.108177200) and GreenBlock1(-0.494601000,1.124870000,-0.093907430) and GreenBlock2(-0.343154100,1.124870000,0.154769500) and BlueBlock1(-0.034629050,1.124870000,-0.401047100) and BlueBlock2(0.095458020,1.124870000,-0.250837300) and PinkBlock1(-0.059464920,1.124870000,-0.133660400) and PinkBlock2(0.551020300,1.124870000,-0.274767400) and YellowBlock1(-0.170534600,1.124870000,-0.375794200) and YellowBlock2(-0.096210390,1.124870000,0.202014500)";
//string focusObj31 = "PinkBlock1";
//string output31 = "the pink block to the right of the green block , the , speech only , (-0.059464920: 1.124870000: -0.133660400)";
//string output_h31 = "focusing on the pink block to the side of the plate , the , speech only ,  (-0.059464920: 1.124870000: -0.133660400)";

////81-82 multi -multi
//string configration32 = "RedBlock1(-0.097416660,1.125127000,-0.152294600) and RedBlock2(-0.110404200,1.124919000,0.275637400) and GreenBlock1(-0.072762650,1.149221000,0.064455520) and GreenBlock2(-0.319034400,1.124870000,0.202901100) and BlueBlock1(-0.302781800,1.124870000,-0.414393800) and BlueBlock2(-0.490903100,1.124870000,0.138156100) and PinkBlock1(0.326105500,1.124870000,-0.408620400) and PinkBlock2(0.375118900,1.124870000,0.219119500) and YellowBlock1(-0.737141800,1.124870000,-0.380026500) and YellowBlock2(-0.737140800,1.124870000,-0.480026300)";
//string focusObj32 = "BlueBlock2";
//string output32 = "lift this blue block , the , multimodal , (-0.490903100: 1.124870000: 0.138156100)";
//string output_h32 = "the blue block to the left of the green block , the , multimodal , (-0.490903100: 1.124870000: 0.138156100)";

////83-84 speech-speech
//string configration33 = "RedBlock1(0.294994700,1.124762000,0.203364800) and RedBlock2(0.294994200,1.124762000,-0.150605500) and GreenBlock1(0.394997900,1.124848000,-0.150632100) and GreenBlock2(-0.540023900,1.124871000,-0.271291500) and BlueBlock1(0.471981100,1.124639000,0.026385200) and BlueBlock2(-0.440023800,1.124870000,-0.271291300) and PinkBlock1(0.118009000,1.124770000,0.026377070) and PinkBlock2(0.017977140,1.124859000,0.026359440) and YellowBlock1(-0.303022400,1.124870000,-0.147000500) and YellowBlock2(-0.334769800,1.124870000,-0.284570700)";
//string focusObj33 = "YellowBlock1";
//string output33 = "pick up the yellow block next to the cup , the , speech only , (-0.303022400: 1.124870000: -0.147000500)";
//string output_h33 = "the yellow block in front , the , speech only ,  (-0.303022400: 1.124870000: -0.147000500)";

////85-86 multi -multi
//string configration34 = "RedBlock1(-0.048623990,1.124870000,0.275963000) and RedBlock2(-0.456140700,1.224870000,0.152492600) and GreenBlock1(0.355498100,1.124870000,-0.285753900) and GreenBlock2(-0.386900100,1.124870000,-0.216193100) and BlueBlock1(-0.199333500,1.124870000,0.399054500) and BlueBlock2(-0.411778400,1.124870000,-0.434177900) and PinkBlock1(0.301401400,1.124870000,0.343263800) and PinkBlock2(-0.456140200,1.124870000,0.152492700) and YellowBlock1(0.301401500,1.224870000,0.343264100) and YellowBlock2(-0.486900300,1.124870000,-0.216193300)";
//string focusObj34 = "YellowBlock2";
//string output34 = "pick up the yellow block next to the green block , the , multimodal , (-0.486900300: 1.124870000: -0.216193300)";
//string output_h34 = "pick up the yellow block , the , multimodal , (-0.486900300: 1.124870000: -0.216193300)";

////87-88  multi-pointing
//string configration35 = "RedBlock1(0.062194690,1.124870000,0.272981400) and RedBlock2(-0.362186600,1.124870000,0.129299400) and GreenBlock1(-0.144703700,1.124870000,-0.358997400) and GreenBlock2(-0.484835700,1.124870000,-0.346286800) and BlueBlock1(-0.096498850,1.124870000,0.013349700) and BlueBlock2(0.367506600,1.124870000,0.245057700) and PinkBlock1(0.062194660,1.224874000,0.272980100) and PinkBlock2(-0.513873300,1.124870000,0.175331800) and YellowBlock1(-0.613874000,1.124870000,0.175331800) and YellowBlock2(-0.287095900,1.124870000,0.370974800) ";
//string focusObj35 = "GreenBlock2";
//string output35 = "No speech , None , pointing only , (-0.484835700: 1.124870000: -0.346286800)";
//string output_h35 = "put this on the yellow block, this , multimodal , (-0.484835700: 1.124870000: -0.346286800)";

////89-90 multi-multi
//string configration36 = "RedBlock1(-0.015303730,1.124870000,0.002760455) and RedBlock2(0.326293200,1.124870000,-0.437550000) and GreenBlock1(0.440192100,1.124870000,0.284377100) and GreenBlock2(0.182971300,1.124870000,-0.357689700) and BlueBlock1(0.068043600,1.124870000,0.272987200) and BlueBlock2(-0.510570300,1.124870000,-0.289925000) and PinkBlock1(0.490037500,1.124870000,-0.371380000) and PinkBlock2(-0.456131800,1.124870000,0.361764900) and YellowBlock1(-0.249550700,1.124870000,-0.184457400) and YellowBlock2(0.355318800,1.124870000,0.404352600)";
//string focusObj36 = "PinkBlock2";
//string output36 = "pick up the pink block next to the cup , the , multimodal , (0.440192100: 1.124870000: 0.284377100)";
//string output_h36 = "the pink block located at the right hand , the , multimodal , (-0.456131800: 1.124870000: 0.361764900)";

////91-92 multi speech
//string configration37 = "RedBlock1(-0.093144420,1.124870000,-0.319331000) and RedBlock2(0.355771600,1.149295000,-0.054836070) and GreenBlock1(-0.032652560,1.124870000,-0.087856800) and GreenBlock2(0.355771200,1.249295000,-0.054836070) and BlueBlock1(0.501877700,1.124870000,0.393093300) and BlueBlock2(-0.054532560,1.124870000,-0.209534300) and PinkBlock1(0.624471800,1.124870000,-0.412015100) and PinkBlock2(-0.585525400,1.124870000,0.227873300) and YellowBlock1(0.570354500,1.124870000,0.195503300) and YellowBlock2(-0.459249500,1.124870000,-0.331685300)";
//string focusObj37 = "YellowBlock2";
//string output37 = "the yellow block to the right of the cup , the , speech only , (-0.459249500: 1.124870000: -0.331685300)";
//string output_h37 = "yellow block , the , multimodal , (-0.459249500: 1.124870000: -0.331685300)";

////93-94 speech point
//string configration38 = "RedBlock1(-0.260244500,1.124870000,-0.315697500) and RedBlock2(-0.215668600,1.124870000,0.250752300) and GreenBlock1(0.123015800,1.124786000,-0.000005692) and GreenBlock2(0.273163000,1.124870000,-0.404861500) and BlueBlock1(-0.044019010,1.124870000,-0.257878500) and BlueBlock2(-0.297323900,1.124870000,-0.175525600) and PinkBlock1(0.021699160,1.124870000,-0.142537900) and PinkBlock2(-0.458309300,1.124870000,-0.286602300) and YellowBlock1(0.533906100,1.124870000,-0.312495400) and YellowBlock2(-0.029290970,1.124870000,0.138387700)";
//string focusObj38 = "RedBlock1";
//string output38 = "No speech , None , pointing only , (-0.260244500: 1.124870000: -0.315697500)";
//string output_h38 = "move the red block to the right of the plate , the , speech only , (-0.260244500: 1.124870000: -0.315697500)";

////95-96 multi multi 
//string configration39 = "RedBlock1(-0.097416660,1.125127000,-0.152294600) and RedBlock2(-0.110404200,1.124919000,0.275637400) and GreenBlock1(-0.072762650,1.149221000,0.064455520) and GreenBlock2(-0.319034400,1.124870000,0.202901100) and BlueBlock1(-0.302781800,1.124870000,-0.414393800) and BlueBlock2(-0.490903100,1.124870000,0.138156100) and PinkBlock1(0.326105500,1.124870000,-0.408620400) and PinkBlock2(0.375118900,1.124870000,0.219119500) and YellowBlock1(-0.737141800,1.124870000,-0.380026500) and YellowBlock2(-0.737140800,1.124870000,-0.480026300)";
//string focusObj39 = "BlueBlock2";
//string output39 = "the blue block , the , multimodal , (-0.490903100: 1.124870000: 0.138156100)";
//string output_h39 = "the blue block to the left of the green block , the , multimodal , (-0.490903100: 1.124870000: 0.138156100)";

////97-98 multi multi
//string configration40 = "RedBlock1(-0.020719290,1.124870000,-0.193172800) and RedBlock2(-0.284995700,1.124870000,-0.249581800) and GreenBlock1(-0.407865100,1.124870000,-0.391886600) and GreenBlock2(0.464595500,1.124870000,-0.263504100) and BlueBlock1(0.083210830,1.124870000,-0.142210700) and BlueBlock2(0.195792000,1.124870000,-0.464375400) and PinkBlock1(0.451640600,1.224870000,-0.441512700) and PinkBlock2(0.107283000,1.124870000,-0.280639600) and YellowBlock1(0.451640100,1.124870000,-0.441512600) and YellowBlock2(0.364595300,1.124870000,-0.263504100)";
//string focusObj40 = "BlueBlock1";
//string output40 = "pick up the blue block next to the red block , the , multimodal , (0.083210830: 1.124870000: -0.142210700)";
//string output_h40 = "the blue block behind the pink block , the , multimodal , (0.083210830: 1.124870000: -0.142210700)";

////99-100 speech speech 
//string configration41 = "RedBlock1(-0.510117200,1.124870000,0.070300100) and RedBlock2(-0.025642990,1.124870000,-0.242599400) and GreenBlock1(0.481602700,1.124870000,-0.194570200) and GreenBlock2(0.294994000,1.124640000,0.203378300) and BlueBlock1(0.233285100,1.124870000,-0.208186800) and BlueBlock2(0.020286260,1.124870000,-0.377099900) and PinkBlock1(0.133964100,1.124870000,0.210130900) and PinkBlock2(-0.500271300,1.124870000,0.288879700) and YellowBlock1(-0.109503600,1.124871000,0.324016900) and YellowBlock2(-0.247528600,1.124870000,-0.374798100)";
//string focusObj41 = "RedBlock1";
//string output41 = "the red block to the left of the pink block , the , speech only , (-0.510117200: 1.124870000: 0.070300100)";
//string output_h41 = "Lift the red block beside the cup , the , speech only ,  (-0.510117200: 1.124870000: 0.070300100)";
