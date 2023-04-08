using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class Tutoriel : MonoBehaviour
{
    public AudioClip[] _audioClips_FR;
    public AudioClip[] _audioClips_EN;
    private AudioClip[] _audioClips;
    public AudioSource TelAudio,BouleBAudio, BouleMAudio;
    public GameObject bouleB, bouleM, nuage, sablier, villageois, palmier, IPad, Ecran, Generique, Bureau, GeneriqueEcran, Tel;
    public GameObject[] plaisir, Villagers;
    public float timetuto, tuto, cas, fin;
    public Vector3 VNuage, VArbre, VVillageois, VSablier, VPlaisir;
    public static float EvNuage, EvCristaux, EvPlaisir;
    [SerializeField] private SwitchSkybox refScriptSwitchSkybox;
    public TempleScript TempleScript;
    public SwitchSkybox Skybox;
    public bool Finjeu;
    public Material Victoire, Defaite, MenuGenerique;
    [SerializeField] private TextMeshProUGUI Helium_fin_text;
    public string[] Commentaires_FR;
    public string[] Commentaires_EN;
    private string[] Commentaires;
    public TextMeshProUGUI IpadCommentaires;

    // Start is called before the first frame update
    void Awake()
    {
      //  _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {

    }
    void Start()
    {

        if(GameManager.instance.GetComponent<GameManager>().Language == "FR")
        {
            Commentaires = Commentaires_FR;
            _audioClips = _audioClips_FR;
        }
        else if (GameManager.instance.GetComponent<GameManager>().Language == "EN")
        {
            Commentaires = Commentaires_EN;
            _audioClips = _audioClips_EN;
        }


        TelAudio.PlayOneShot(_audioClips[44]);
        Tel.gameObject.SetActive(true);

    }

    IEnumerator tracking_balls()
    {
        while(tuto>=6)
        {

            VNuage = nuage.transform.position;
            VSablier = sablier.transform.position;


            yield return new WaitForFixedUpdate();
        }
    }

    public void TutoCristaux()
    {
      if(EvCristaux ==0) StartCoroutine(TutoEventCristaux());
    }
    public  void TutorielNuage()
    {
        if (EvNuage == 0) StartCoroutine(TutoEventNuages());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BMParle()
    { bouleM.GetComponent<Animator>().Play("happy");
      bouleB.GetComponent<Animator>().Play("flotte");
    }
    public void BBParle()
    {
        bouleB.GetComponent<Animator>().Play("happy");
        bouleM.GetComponent<Animator>().Play("flotte");
    }
    public void DecrocheTelephone()
    {
       
        if (tuto == 0 && fin == 0)
        {
            TelAudio.Stop();
            StartCoroutine(DebutTelephone());

        }
        else if (fin > 0 && Finjeu == false)
        {
            TelAudio.Stop();
            StartCoroutine(FIN());
        }
       
    }
    public void RaccrocheTelephone()
    {
        //FIN TUTO
        TelAudio.Stop();
        if (fin == 0)
        {
            tuto = 6;
            // Faire disparaitre après quelques secondes
            StopAllCoroutines();
            StartCoroutine(DisparitionTel(0));
            
        }

    }
    public IEnumerator DisparitionTel(float time)
    {
        while (time < 10)
        {
            Debug.Log("Temps :" + time);
            time++;
            yield return new WaitForSeconds(1);
        }
        Tel.gameObject.SetActive(false);
    }

    void NuagePos()
    {
        VNuage = nuage.transform.position;
        VNuage.y += 5;
    }
    void SablierPos()
    {
        VSablier = sablier.transform.position;
        VSablier.y += 10;
    }
    void ArbrePos()
    {
        VArbre = palmier.transform.position;
        VArbre.y += 10;
    }
    void PlaisirPos()
    {
        VPlaisir = plaisir[1].transform.position;
        VPlaisir.y += 10;
    }
    void VillageoisPos()
    {
        VVillageois = villageois.transform.position;
        VVillageois.y += 10;
    }



    public IEnumerator DebutTelephone()
    {
        tuto = 1;
        TelAudio.PlayOneShot(_audioClips[0]); //voix telephone
        while (TelAudio.isPlaying) yield return null;
        yield return new WaitForSeconds(2f);

        //boules apparaissent
        ApparitionBoules();
        Boules.speed1 = 5f;

        BouleMAudio.PlayOneShot(_audioClips[1]); //eh attention !! 
        yield return new WaitForSeconds(3f);
        //les boules arrivent rapidement vers le joueur, le contournent, font le tour de l'ile
        Boules.speed1 = 0.5f;
        //lancer animation BouleM
        bouleM.GetComponent<Animator>().Play("panique");
        BouleMAudio.PlayOneShot(_audioClips[2]);
        while (BouleMAudio.isPlaying) yield return null;

        BBParle();
        BouleBAudio.PlayOneShot(_audioClips[3]);
        while (BouleBAudio.isPlaying) yield return null;

        bouleM.GetComponent<Animator>().Play("panique");
        bouleB.GetComponent<Animator>().Play("flotte");
        BouleMAudio.PlayOneShot(_audioClips[4]);
        while (BouleMAudio.isPlaying) yield return null;

        BBParle();
        BouleBAudio.PlayOneShot(_audioClips[5]);
        while (BouleBAudio.isPlaying) yield return null;
        bouleB.GetComponent<Animator>().Play("flotte");


        // Commencement tuto nuages bouleB va vers arbre et tourne 
        StartCoroutine(tracking_balls());
        bouleB.GetComponent<Boules>().deplacement = true;
        ArbrePos();
        while (timetuto < 3f)
        {
            bouleB.transform.position = Vector3.MoveTowards(bouleB.transform.position, VArbre, 20 * Time.deltaTime);
            timetuto += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        timetuto = 0f;
        bouleB.GetComponent<Animator>().Play("happy");
        BouleBAudio.PlayOneShot(_audioClips[6]);
        while (BouleBAudio.isPlaying) yield return null;
        bouleB.GetComponent<Animator>().Play("flotte");

        //bouleB se deplace vers les nuages 
        NuagePos();
        while (timetuto < 4f)
        {
            bouleB.transform.position = Vector3.MoveTowards(bouleB.transform.position, VNuage, 20 * Time.deltaTime);
            timetuto += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        timetuto = 0f;
        bouleB.GetComponent<Animator>().Play("happy");

        tuto = 2;
    }
    //REMPLACER WAIT PAR LACTION DU JOUEUR
    public void TutoNuage()
    {
        //Debug.Log("TutoNuage");
        if(tuto == 2) StartCoroutine(PickNuageTuto());
    }

    public IEnumerator PickNuageTuto()
    {
        //joueur prend nuage, boule revient vers arbre
        tuto = 3;
        bouleB.GetComponent<Animator>().Play("flotte");
        ArbrePos();
        while (timetuto < 4f)
        {
            bouleB.transform.position = Vector3.MoveTowards(bouleB.transform.position, VArbre, 20 * Time.deltaTime);
            timetuto += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        timetuto = 0f;
        bouleB.GetComponent<Animator>().Play("happy");
        //REMPLACER WAIT PAR LACTION DU JOUEUR
        yield return new WaitForSeconds(3f);
        //joueur doit placer nuage au-dessus du champs
        bouleB.GetComponent<Animator>().Play("happy");
        BouleBAudio.PlayOneShot(_audioClips[7]);
        while (BouleBAudio.isPlaying) yield return null;
        bouleB.GetComponent<Animator>().Play("flotte");
        //"ça devrait faire l'affaire", joueur presse le nuage 
        yield return new WaitForSeconds(2f);
        bouleM.GetComponent<Animator>().Play("happy");
        BouleBAudio.PlayOneShot(_audioClips[8]);
        while (BouleBAudio.isPlaying) yield return null;
        BouleBAudio.PlayOneShot(_audioClips[9]);
        bouleM.GetComponent<Animator>().Play("flotte");
        bouleB.GetComponent<Animator>().Play("panique");
        while (BouleBAudio.isPlaying) yield return null;
        BMParle();
        BouleBAudio.PlayOneShot(_audioClips[10]);
        while (BouleBAudio.isPlaying) yield return null;
        bouleM.GetComponent<Animator>().Play("flotte");
        //boule retourne vers joueur
        bouleB.GetComponent<Boules>().deplacement = false;
        yield return new WaitForSeconds(2f);
        //TUTO sablier
        BouleBAudio.PlayOneShot(_audioClips[11]);
        while (BouleBAudio.isPlaying) yield return null;
        bouleB.GetComponent<Boules>().deplacement = true;
        SablierPos();
        while (timetuto < 4f)
        {
            bouleB.transform.position = Vector3.MoveTowards(bouleB.transform.position, VSablier, 20 * Time.deltaTime);
            timetuto += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        timetuto = 0f;
        bouleB.GetComponent<Animator>().Play("tourne");
        BouleBAudio.PlayOneShot(_audioClips[12]);
        while (BouleBAudio.isPlaying) yield return null;
        bouleB.GetComponent<Animator>().Play("flotte");  
        tuto = 4;
    }
    public void SablierTuto()
    {
        Debug.Log("TutoSablier");
        if (tuto==4) StartCoroutine(PickSablierTuto());

    }
    public IEnumerator PickSablierTuto()
    {
        tuto = 5;
        // joueur doit prendre sablier
        yield return new WaitForSeconds(2f);
        //jai retire la position du villageois ca risque de poser pb

        //boule va vers un villageois
        //villageois.gameObject.SetActive(true);
        //VillageoisPos();
        //while (timetuto < 4f)
        //{
        //    bouleB.transform.position = Vector3.MoveTowards(bouleB.transform.position, VVillageois, 20 * Time.deltaTime);
        //    timetuto += 0.01f;
        //    yield return new WaitForSeconds(0.01f);
        //}
        //bouleB.GetComponent<Animator>().Play("happy");
        //timetuto = 0f;

        ////joueur renverse sablier sur villageois 
        //yield return new WaitForSeconds(2f);
        //bouleB.GetComponent<Animator>().Play("flotte");
        //bouleM.GetComponent<Animator>().Play("happy");
        //BouleMAudio.PlayOneShot(_audioClips[13]);
        //while (BouleMAudio.isPlaying) yield return null;
        bouleB.GetComponent<Boules>().deplacement = false;
        bouleM.GetComponent<Animator>().Play("flotte");

        //si le joueur sexecute le villageois s'envole et oups c'est pas moi
        yield return new WaitForSeconds(2f);

        BBParle();
        BouleBAudio.PlayOneShot(_audioClips[14]);
        while (BouleBAudio.isPlaying) yield return null;
        //MODIFICATIONS la nuit tombe
        villageois.gameObject.SetActive(false);
        BMParle();
        BouleMAudio.PlayOneShot(_audioClips[15]);
        while (BouleMAudio.isPlaying) yield return null;
        //plus ils en récolteront plus nous seront riches ! 
        //partie temple prieres 
        BBParle();
        BouleBAudio.PlayOneShot(_audioClips[16]);
        while (BouleBAudio.isPlaying) yield return null;
        BMParle();
        BouleMAudio.PlayOneShot(_audioClips[17]);
        while (BouleMAudio.isPlaying) yield return null;
        BBParle();
        BouleBAudio.PlayOneShot(_audioClips[18]);
        while (BouleBAudio.isPlaying) yield return null;
        BMParle();
        BouleMAudio.PlayOneShot(_audioClips[19]);
        while (BouleMAudio.isPlaying) yield return null;
        bouleM.GetComponent<Animator>().Play("flotte");
        //tuto yoga time
        yield return new WaitForSeconds(3f);

        //byebye
        BMParle();
        BouleMAudio.PlayOneShot(_audioClips[20]);
        while (BouleMAudio.isPlaying) yield return null;
        BBParle();
        BouleBAudio.PlayOneShot(_audioClips[21]);
        while (BouleBAudio.isPlaying) yield return null;
        bouleB.GetComponent<Animator>().Play("flotte");
        //boules disparaissent
        DisparitionBoules();
        tuto = 6;
    }


    //TUTORIEL EVENEMENT NUAGES
    public IEnumerator TutoEventNuages()
    {
        ApparitionBoules();
        yield return new WaitForSeconds(2f);
        BouleBAudio.PlayOneShot(_audioClips[22]);
        bouleB.GetComponent<Animator>().Play("happy");
        bouleM.GetComponent<Animator>().Play("happy");
        while (BouleBAudio.isPlaying) yield return null;
        bouleB.GetComponent<Animator>().Play("flotte");
        bouleM.GetComponent<Animator>().Play("flotte");
        yield return new WaitForSeconds(8f);
        //boules se dirigent vers nuages en animation panique _ peut etre a retirer
        //si joueur agrippe nuage 
        // BouleBAudio.PlayOneShot(_audioClips[23]);

        DisparitionBoules();
    }

    //TUTORIEL EVENEMENT CRISTAUX
    public IEnumerator TutoEventCristaux()
    {
        ApparitionBoules();
        yield return new WaitForSeconds(2f);
        BBParle();
        BouleBAudio.PlayOneShot(_audioClips[24]);
        while (BouleBAudio.isPlaying) yield return null;
        BMParle();
        BouleBAudio.PlayOneShot(_audioClips[25]);
        while (BouleBAudio.isPlaying) yield return null;
        bouleM.GetComponent<Animator>().Play("flotte");

        //si joueur lance éclair 
        BouleBAudio.PlayOneShot(_audioClips[23]);

        DisparitionBoules();
    }

   

    public IEnumerator TropBonheur()
    {
        ApparitionBoules();
        yield return new WaitForSeconds(3f);
        BouleMAudio.PlayOneShot(_audioClips[26]);
        while (BouleMAudio.isPlaying) yield return null;

        //boules se dirigent vers bat plaisir         
        bouleM.GetComponent<Boules>().deplacement = true;
        bouleB.GetComponent<Boules>().deplacement = true;
        //le batiment plaisir doit avoir le tag plaisir
        PlaisirPos();
        while (timetuto < 3f)
        {
            bouleM.transform.position = Vector3.MoveTowards(bouleM.transform.position, VPlaisir, 20 * Time.deltaTime);
            bouleB.transform.position = Vector3.MoveTowards(bouleB.transform.position, VPlaisir, 20 * Time.deltaTime);
            timetuto += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        BBParle();
        BouleBAudio.PlayOneShot(_audioClips[27]);
        while (BouleBAudio.isPlaying) yield return null;
        BMParle();
        BouleMAudio.PlayOneShot(_audioClips[28]);
        while (BouleMAudio.isPlaying) yield return null;
        bouleM.GetComponent<Boules>().deplacement = false;
        bouleB.GetComponent<Boules>().deplacement = false;
        yield return new WaitForSecondsRealtime(5);

        //joueur detruit le bat ?
        if (plaisir.Length <= 0)
        {
            BMParle();
            BouleMAudio.PlayOneShot(_audioClips[29]);
            while (BouleMAudio.isPlaying) yield return null;
           
        }

        bouleM.GetComponent<Animator>().Play("flotte");
        DisparitionBoules();
        yield return null;
    }



    // PLUS DE VILLAGEOIS FIN 1
    public void PlusdeVillageois()
    {
        Time.timeScale = 1;
        fin = 1;
        tuto = 6;
        bouleB.GetComponent<Boules>().deplacement = false;
        bouleM.GetComponent<Boules>().deplacement = false;
        BouleMAudio.Stop();
        BouleBAudio.Stop();
        DisparitionBoules();
        StopGame();
       // SI JOUEUR DECROCHE StartCoroutine(FIN());
    }  

    //A FAIRE APPARAITRE A LA TOMBEE DE LA NUIT LE DERNIER JOUR 
    public IEnumerator DernierJour()
    {
        tuto = 6;
        TempleScript.instance.gameObject.GetComponent<YogaScript>().activate_yoga_hand(0, false);
        Time.timeScale = 1;
       // ApparitionBoules();
       // if (TempleScript.village_terror < 50 && TempleScript.essence_num >= 100000) cas = 1;
       // if (TempleScript.village_terror >= 50 && TempleScript.essence_num >= 100000) cas = 2;
       // if (TempleScript.village_terror < 50 && TempleScript.essence_num < 100000) cas = 3;
       // if (TempleScript.village_terror >= 50 && TempleScript.essence_num < 100000) cas = 4;
       // BouleBAudio.Stop();
       // BouleMAudio.Stop();

       // //if dernier jour cas 1
       // if (cas == 1)
       // {
       //     BouleBAudio.PlayOneShot(_audioClips[33]);
       //     while (BouleBAudio.isPlaying) yield return null;
       //     BouleMAudio.PlayOneShot(_audioClips[34]);
       //     while (BouleMAudio.isPlaying) yield return null;
       // }
       // //if dernier jour cas 2
       // if(cas == 2)
       // {
       //     BouleMAudio.PlayOneShot(_audioClips[35]);
       //     while (BouleMAudio.isPlaying) yield return null;
       //     BouleBAudio.PlayOneShot(_audioClips[36]);
       //     while (BouleBAudio.isPlaying) yield return null;
       //     BouleMAudio.PlayOneShot(_audioClips[37]);
       //     while (BouleMAudio.isPlaying) yield return null;
       // }
       // //if dernier jour cas 3
       // if (cas == 3)
       // {
       //     BouleBAudio.PlayOneShot(_audioClips[38]);
       //     while (BouleBAudio.isPlaying) yield return null;
       //     BouleMAudio.PlayOneShot(_audioClips[39]);
       //     while (BouleMAudio.isPlaying) yield return null;
       // }
       // //if dernier jour cas 4
       //if (cas == 4)
       // {
       //     BouleMAudio.PlayOneShot(_audioClips[40]);
       //     while (BouleMAudio.isPlaying) yield return null;
       //     BouleBAudio.PlayOneShot(_audioClips[41]);
       //     while (BouleBAudio.isPlaying) yield return null;
       // }

        if (TempleScript.essence_num >= 100000) SetFinVictoire();
        else SetFinDefaite();
        yield return null;
    }

    
    public void SetFinVictoire()
    {
        fin = 2;
        StopGame();
    }

    public void SetFinDefaite()
    {
        fin = 3;
        StopGame();
    }
    public IEnumerator FIN()
    {
        Finjeu = true;
        //fin 1 = plus de villageois
        if (fin == 1)
        {
            TelAudio.PlayOneShot(_audioClips[30]); //voix telephone
            while (TelAudio.isPlaying) yield return null;
            ApparitionBoules();
            BouleMAudio.PlayOneShot(_audioClips[31]);
            while (BouleMAudio.isPlaying) yield return null;
            BouleBAudio.PlayOneShot(_audioClips[32]);
            while (BouleBAudio.isPlaying) yield return null;
        }
        //fin 2 = c'est gagné !
        else if (fin == 2) TelAudio.PlayOneShot(_audioClips[42]); //voix telephone       
        //fin 2 = c'est perdu !
        else if (fin == 3) TelAudio.PlayOneShot(_audioClips[43]); //voix telephone

        //Génération des commentaires aléatoires en fonction de l'alignement
        if (TempleScript.village_terror < 30) IpadCommentaires.text = Commentaires[Random.Range(0,22)];
        else if (TempleScript.village_terror >= 30 && TempleScript.village_terror < 60) IpadCommentaires.text = Commentaires[Random.Range(23, 37)];
        else if (TempleScript.village_terror >= 60) IpadCommentaires.text = Commentaires[Random.Range(37, 65)];

        //monde qui devient blanc
        DisparitionBoules();
        SetEndGame();

        //menu qui s'affiche
        // générique qui se lance

        yield return null;
    }



    public void ApparitionBoules()
    {
        bouleB.gameObject.SetActive(true);
        bouleM.gameObject.SetActive(true);
    }
    public void DisparitionBoules()
    {
        bouleB.gameObject.SetActive(false);
        bouleM.gameObject.SetActive(false);
    }

    public void SetEndGame()
    {
        StartCoroutine(Skybox.TransitionSkybox());
        StartCoroutine(WaitMenu(0));
    }

    public IEnumerator WaitMenu(float TimeMenu)
    {
        IPad.gameObject.SetActive(true);
        //menu qui s'affiche
        if (fin == 1 | fin == 3) Ecran.GetComponent<MeshRenderer>().material = Defaite;
        if (fin == 2) Ecran.GetComponent<MeshRenderer>().material = Victoire;
        while (TimeMenu <5)
        {
            TimeMenu++;
            yield return new WaitForSeconds(5);
        }
        // générique qui se lance
        RenderSettings.ambientIntensity = 1f;
        GeneriqueEcran.gameObject.SetActive(true);
        Bureau.gameObject.SetActive(false);
        Generique.gameObject.SetActive(true);
        // on desactive le menu de victoire et on le remplace par le generique ? ou on fait apparaitre le generique a cote ou un truc du genre ?
    }
    public void StopGame()
    {
        Debug.Log("FIN DU JEU");

        Helium_fin_text.text = TempleScript.instance.essence_num.ToString();

        TempleScript.instance.transform.parent.gameObject.SetActive(false);
        //Time.timeScale = 0;
        Tel.gameObject.SetActive(true);
        TelAudio.PlayOneShot(_audioClips[44]); //a remplacer par la sonnerie       
    }

}
