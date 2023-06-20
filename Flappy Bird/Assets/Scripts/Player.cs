using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;
    private Vector3 direction;
    public float gravity = -9.8f;
    public float strenght = 5f;
    public GameManager gameManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite),0.15f,0.15f);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strenght;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                direction = Vector3.up * strenght;
            }
        }
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    private void AnimateSprite()
    {
        spriteIndex++;
        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle" || transform.position.y <= -4)
        {
            gameManager.GameOver();
            SceneManager.LoadScene("Menu");
        }
        else if (other.gameObject.tag == "Scoring" ) 
        {
            FindObjectOfType<GameManager>().IncreaseScore();
        }
    }

}
