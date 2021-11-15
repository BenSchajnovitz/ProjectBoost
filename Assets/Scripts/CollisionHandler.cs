using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip levelFinished;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem levelFinishedParticle;
    [SerializeField] ParticleSystem deathParticle;
     

    bool collisionDisabled = false;
    bool isTransitioning = false;

    AudioSource audioSource;
    Movement movement;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<Movement>();
    }

    void Update() 
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            NextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
        }
    }

    void OnCollisionEnter(Collision other) {
        
        if(isTransitioning || collisionDisabled) { return; }

        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Touched Friendly");
                break;

            case "Fuel":
                Debug.Log("Touched Fuel");
                break;

            case "Finish":
                LevelFinished();
                break;       
            default:
                GameOver();
                break;

        }
    }

    void GameOver()
    {
        isTransitioning = true;
        audioSource.Stop();
        deathParticle.Play();
        audioSource.PlayOneShot(death);
        movement.enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void LevelFinished()
    {
        isTransitioning = true;
        audioSource.Stop();
        levelFinishedParticle.Play();
        audioSource.PlayOneShot(levelFinished);
        movement.enabled = false;
        Invoke("NextLevel", levelLoadDelay);
    }

    void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1 ) % SceneManager.sceneCountInBuildSettings;

        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
