using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Player : MonoBehaviour
{
    public AudioClip godModeSound;
    public AudioClip jumpSound1, jumpSound2, springSound;
    public AudioClip robloxSound, tomscreamSound, succSound;
    public AudioClip tpSound, getCoinSound, loseCoinSound;

    bool godMode, bMoving, bGrounded, bDJumping;

    float speed, jumpHeight, gravity, ySpeed;
    bool canRotate, rotated;
    Vector3 currentDestination, nextDestination;

    //Monedas:
    public int coins;
    public TextMeshProUGUI coinsDisplay;

    //Giros:
    public int turns;
    public TextMeshProUGUI turnsDisplay;

    //Puntuacion
    [SerializeField] private int score;
    [SerializeField] private int highscore;
    public TextMeshProUGUI highscoreDisplay;

    bool spacePressed;

    [SerializeField] private Chomp chomp;

    void Start()
    {
        resetPlayer();
    }

    public void resetPlayer()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        godMode = false;
        bMoving = false;
        bGrounded = true;
        bDJumping = false;

        speed = 12.0f;
        jumpHeight = 3.5f;
        gravity = -55.0f;
        ySpeed = 0;
        canRotate = false;
        rotated = false;
        currentDestination = transform.position + 3 * 4 * Vector3.forward;
        currentDestination += 16.0f * (currentDestination - transform.position).normalized;

        coins = 0;
        turns = 0;
        score = 0;

        spacePressed = true;

        chomp.resetChomp();
    }

    void loseGame()
    {
        //Actualizamos puntuación:
        if (score > highscore) highscore = score;
        highscoreDisplay.text = highscore.ToString();

        GameManager.Instance.ChangeState(GameState.Lose);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            godMode = !godMode;
            if (godMode) AudioSource.PlayClipAtPoint(jumpSound1, transform.position);
        }

        if (GameManager.Instance.State == GameState.Play)
        {
            coinsDisplay.text = coins.ToString();
            turnsDisplay.text = turns.ToString();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bMoving = false;
                chomp.stopChomp();
                //Cambiar estado a menu de pausa.
                GameManager.Instance.ChangeState(GameState.PauseMenu);
            }

            if (bMoving)
            {
                if (godMode && canRotate && turns != 59)
                {
                    canRotate = false;
                    rotated = true;

                    //Cambiamos dirección.
                    currentDestination = nextDestination + 16.0f * (nextDestination - transform.position).normalized;

                    //Rotamos player.
                    transform.rotation = Quaternion.LookRotation(currentDestination);

                    ++turns;
                    ++score;
                }
                else
                {
                    if (spacePressed && !Input.GetKey(KeyCode.Space)) spacePressed = false;
                    if (bGrounded)
                    {
                        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                        bDJumping = false;
                        ySpeed = 0;
                    }
                    else ySpeed += gravity * Time.deltaTime;

                    if (Input.GetKey(KeyCode.Space) && !spacePressed)
                    {
                        spacePressed = true;

                        if (canRotate)
                        {
                            canRotate = false;
                            rotated = true;

                            //Cambiamos dirección.
                            currentDestination = nextDestination + 16.0f * (nextDestination - transform.position).normalized;

                            //Rotamos player.
                            transform.rotation = Quaternion.LookRotation(currentDestination);

                            ++turns;
                            ++score;
                        }
                        else if (bGrounded)
                        {
                            bGrounded = false;
                            bDJumping = false;
                            ySpeed = Mathf.Sqrt(jumpHeight * (-2) * gravity);

                            AudioSource.PlayClipAtPoint(jumpSound1, transform.position);
                        }
                        else if (!bDJumping)
                        {
                            bDJumping = true;
                            ySpeed = Mathf.Sqrt(jumpHeight * (-2) * gravity);

                            AudioSource.PlayClipAtPoint(jumpSound2, transform.position);
                        }
                        else spacePressed = false;
                    }
                }
                //Desplazamiento
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentDestination.x, transform.position.y, currentDestination.z), speed * Time.deltaTime);
                transform.Translate(new Vector3(0, ySpeed, 0) * Time.deltaTime);
            }
            else
            {
                bMoving = true;
                //AudioSource.PlayClipAtPoint(jumpSound1, transform.position);
            }

            //Perder.
            if (transform.position.y <= -14.0f)
            {
                AudioSource.PlayClipAtPoint(tomscreamSound, transform.position);
                loseGame();
            }
        }
    }

    //Collision
    [SerializeField] private GameObject TurnedBlock;

    void OnTriggerEnter(Collider objeto)
    {
        if (objeto.gameObject.CompareTag("Coin"))
        {
            Destroy(objeto.gameObject);
            AudioSource.PlayClipAtPoint(getCoinSound, transform.position);
            ++coins;
            score += 2;
        }
        else if (objeto.gameObject.CompareTag("Chomp"))
        {
            if (!godMode) loseGame();
        }
        else
        {
            bGrounded = ySpeed <= 0;
            if (bGrounded)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                bDJumping = false;
                ySpeed = 0;
            }

            if (objeto.gameObject.CompareTag("TurnBlock"))
            {
                //Si pisamos un TurnBlock, almacenamos y permitimos ir en dirección a la siguiente.
                canRotate = true;

                nextDestination = objeto.gameObject.GetComponent<TurnBlock>().getNextDestination();
            }
            else if (objeto.gameObject.CompareTag("KillerTrap1"))
            {
                AudioSource.PlayClipAtPoint(robloxSound, transform.position);
                if (!godMode) loseGame();
            }
            else if (objeto.gameObject.CompareTag("SlowTrap2"))
            {
                AudioSource.PlayClipAtPoint(succSound, transform.position);
                if (!godMode) speed = 4.0f;
            }
            else if (objeto.gameObject.CompareTag("TpTrap3"))
            {
                AudioSource.PlayClipAtPoint(tpSound, transform.position);
                if (!godMode) transform.position -= 4 * (currentDestination - transform.position).normalized;
            }
            else if (objeto.gameObject.CompareTag("MoneyTrap4"))
            {
                AudioSource.PlayClipAtPoint(loseCoinSound, transform.position);

                if (!godMode)
                {
                    if (coins >= 2)
                    {
                        coins -= 2;
                        score -= 4;
                    }
                    else if (coins == 1)
                    {
                        coins = 0;
                        if (score >= 5) score -= 5;
                        else score = 0;
                    }
                    else if (score >= 6) score -= 6;
                    else score = 0;
                }
            }
            else if (objeto.gameObject.CompareTag("JumpTrap5"))
            {
                AudioSource.PlayClipAtPoint(springSound, transform.position);

                if (!godMode)
                {
                    bGrounded = false;
                    bDJumping = true;
                    ySpeed = Mathf.Sqrt((jumpHeight + 2) * (-2) * gravity);
                }
            }
        }
    }

    void OnTriggerExit(Collider objeto)
    {
        if (!objeto.gameObject.CompareTag("Coin"))
        {
            bGrounded = false;

            if (objeto.gameObject.CompareTag("TurnBlock"))
            {
                //Si dejamos de pisar un TurnBlock, dejamos de permitir ir en dirección a la siguiente.
                canRotate = false;
            }
            else if (objeto.gameObject.CompareTag("SlowTrap2"))
            {
                speed = 12.0f;
            }
            else if (objeto.gameObject.CompareTag("blockwin"))
            {
                GameManager.Instance.ChangeState(GameState.Win);
            }
        }
    }

    void OnTriggerStay(Collider objeto)
    {
        if (!objeto.gameObject.CompareTag("Coin"))
        {
            bGrounded = ySpeed <= 0;
            if (bGrounded)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                bDJumping = false;
                ySpeed = 0;
            }
        }
        if (objeto.gameObject.CompareTag("TpTrap3"))
        {
            if (!godMode) transform.position -= 0.5f * (currentDestination - transform.position).normalized;
        }
        else if (rotated && objeto.gameObject.CompareTag("TurnBlock"))
        {
            //Si estamos en un TurnBlock y giramos, lo desactivamos y spawneamos TurnedBlock.
            rotated = false;

            Vector3 nd = objeto.gameObject.GetComponent<TurnBlock>().getNextDestination();
            Vector3 posBloque = objeto.transform.position;
            Destroy(objeto.gameObject);
            Transform lvlTransform = GameObject.FindGameObjectWithTag("Level").transform;
            GameObject newTurnedBlock = Instantiate(TurnedBlock, posBloque, Quaternion.identity, lvlTransform);
            newTurnedBlock.gameObject.GetComponent<TurnedBlock>().setNextDestination(nd);
        }
    }
}