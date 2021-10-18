using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eyeTypes
{
    GREEN = 0,
    YELLOW = 1,
    RED = 2,
    BOMB = 3
}

[RequireComponent(typeof(CircleCollider2D))]

public class InflateMechanics : MonoBehaviour
{
    [HideInInspector] public Dictionary<Vector2, bool> fullDictionary = new Dictionary<Vector2, bool>();

    [Header("����������� ��������� ����������� ����")]
    [SerializeField] private int ProbGreen = 50;
    [SerializeField] private int ProbYellow = 40;
    [SerializeField] private int ProbRed = 5;
    [SerializeField] private int ProbBomb = 5;

    [Header("������� �������� ������� �����")]
    [SerializeField] private Sprite[] spritesEYE;

    [Header("������� ������")]
    [SerializeField] private Sprite[] spritesPUPIL;

    private eyeTypes _eyeType;

    private float inflateSpeed = 0;
    private float destroyTime = 1.0F;

    private float upTime = 0.0F;
    private float startTime = 0.0F;

    private int rand;
    private int currentStage = 0;

    private SpriteRenderer SR;
    private SpriteRenderer SRPupil;
    private Vector2 parentPosition;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        SR = GetComponent<SpriteRenderer>();
        SRPupil = transform.GetChild(0).GetComponent<SpriteRenderer>();
        parentPosition = (Vector2)transform.parent.position;


        GenerateEyeType();
        SetParams(_eyeType);
    }

    // Update is called once per frame
    void Update()
    {
        if (upTime > destroyTime)
        {
            fullDictionary[parentPosition] = false;
            Destroy(transform.parent.gameObject);
        }

        if (_eyeType != eyeTypes.BOMB)
        {
            if (upTime > 0.3F * destroyTime && upTime < 0.6F * destroyTime && currentStage == 0)
            {
                SR.sprite = spritesEYE[1];
                currentStage = 1;
            }
            else if (upTime > 0.6F * destroyTime && currentStage == 1)
            {
                SR.sprite = spritesEYE[2];
                currentStage = 2;
            }
        }

    }

    private void FixedUpdate()
    {
        transform.localScale += transform.localScale.normalized*inflateSpeed;
        upTime = Time.time - startTime;
    }

    private void GenerateEyeType()
    {
        rand = Random.Range(0, 100);

        if (rand > 100 - ProbGreen)
        {
            _eyeType = eyeTypes.GREEN;
        }
        else if (rand > 100 - ProbGreen - ProbYellow && rand <= 100 - ProbGreen)
        {
            _eyeType = eyeTypes.YELLOW;
        }
        else if (rand > 100 - ProbGreen - ProbYellow - ProbRed && rand <= 100 - ProbGreen - ProbYellow)
        {
            _eyeType = eyeTypes.RED;
        }
        else if (rand <= ProbBomb)
        {
            _eyeType = eyeTypes.BOMB;
        }
    }

    private void SetParams(eyeTypes eT)
    {
        switch (eT)
        {
            case eyeTypes.GREEN:
                SR.sprite = spritesEYE[0];
                SRPupil.sprite = spritesPUPIL[0];
                destroyTime = 2.0F;
                inflateSpeed = 0.01F;
                break;
            case eyeTypes.YELLOW:
                SR.sprite = spritesEYE[0];
                SRPupil.sprite = spritesPUPIL[1];
                destroyTime = 1.0F;
                inflateSpeed = 0.02F;
                break;
            case eyeTypes.RED:
                SR.sprite = spritesEYE[0];
                SRPupil.sprite = spritesPUPIL[2];
                destroyTime = 0.5F;
                inflateSpeed = 0.04F;
                break;
            case eyeTypes.BOMB:
                SR.sprite = spritesEYE[3];
                rand = Random.Range(0, 3);
                SRPupil.sprite = spritesPUPIL[rand];
                destroyTime = 1.0F;
                inflateSpeed = 0.02F;
                break;
        }
    }

    public int GetPoints()
    {
        int points = 0;
        switch (_eyeType) {
            case eyeTypes.GREEN:
                points = (int)(100 * upTime / destroyTime);
                break;
            case eyeTypes.YELLOW:
                points = 100 + (int)(100 * upTime / destroyTime);
                break;
            case eyeTypes.RED:
                points = 200 + (int)(200 * upTime / destroyTime);
                break;
            case eyeTypes.BOMB:
                points = -1000;
                break;
        }

        return points;
    }

}
