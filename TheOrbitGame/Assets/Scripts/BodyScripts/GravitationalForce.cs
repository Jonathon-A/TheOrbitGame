using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravitationalForce : MonoBehaviour
{


    public Vector2 InitialVelocity;

    private float GravitationalConstant = 66.7408f;
    private Rigidbody2D ThisBody;

    public int BodyHealth;
    public bool BodyInvincible;
    public int BodyDamage;

    public LineRenderer IntitialLR;
    private LineRenderer LR;

    public void InitialiseBody(Vector2 StartVelocity)
    {
        ThisBody = gameObject.GetComponent<Rigidbody2D>();
        FutureVelocity = StartVelocity;
        FuturePosition = ThisBody.position;
        TrajectoryQueue.Clear();

        CollisionDetected = false;
        IsDestroyed = false;


        Health = BodyHealth;
        Invincible = BodyInvincible;
        Damage = BodyDamage;

    }


    public void InitialiseLine()
    {

        LR = Instantiate(IntitialLR, gameObject.transform.position, Quaternion.identity);
        //LR.tag = "PredictionLine";
        Color NewColor = gameObject.GetComponent<SpriteRenderer>().color;
        LR.startColor = NewColor;
        NewColor.a = 0.5f;
        LR.endColor = NewColor;
        // LR.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.3f));
        LR.positionCount = 0;

    }

    public GameObject IntitialPredictionBody;
    private GameObject PredictionBody;
    private SpriteRenderer PredictionBodySprite;
    private Rigidbody2D PredictionBodyPhysics;
    private ContactFilter2D ColliderFilter;

    public void InitialisePredictionBody()
    {

        PredictionBody = Instantiate(IntitialPredictionBody);

        PredictionBody.transform.localScale = ThisBody.transform.localScale;
        PredictionBody.transform.position = new Vector3(ThisBody.transform.position.x, ThisBody.transform.position.y, 1);
        PredictionBody.name = "Prediction " + gameObject.name;

        SpriteRenderer BodySprite = gameObject.GetComponent<SpriteRenderer>();

        SpriteRenderer BodySpriteCopy = PredictionBody.GetComponent<SpriteRenderer>();

        BodySpriteCopy.sprite = BodySprite.sprite;
        Color NewColor = BodySprite.color;
        NewColor.a = 0.5f;
        BodySpriteCopy.color = NewColor;

        //BodySpriteCopy.material = BodySprite.material;

        PredictionBodySprite = BodySpriteCopy;

        PredictionBodyPhysics = PredictionBody.GetComponent<Rigidbody2D>();

        ColliderFilter = new ContactFilter2D();
        ColliderFilter.useTriggers = true;
        ColliderFilter.minDepth = 0.5f;
        ColliderFilter.maxDepth = 1.5f;
        ColliderFilter.useDepth = true;

        PredictionBody.GetComponent<PreviewProperties>().SetDamage(Damage);


    }

    //private GameObject ThisSprite;
    // private SpriteRenderer ThisSpriteRenderer;








    private Queue<TrajectoryProperties> TrajectoryQueue = new Queue<TrajectoryProperties>();
    private Vector2 FutureVelocity;
    private Vector2 FuturePosition;

    public void ResetTrajectoryQueue()
    {
        //FutureVelocity = TrajectoryQueue.Peek().getVelocity();
        //FuturePosition = TrajectoryQueue.Peek().getPosition();
        FutureVelocity = CurrentTrajectoryProperties.getVelocity();
        FuturePosition = CurrentTrajectoryProperties.getPosition();
        TrajectoryQueue.Clear();

    }

    public void CalculateFutureVelocity()
    {
        if (!CollisionDetected)
        {
            List<BodyProperties> AllBodies = BodiesController.GetAllBodies();

            Vector2 ResultantAcceleration = new Vector2();

            for (int i = 0; i < AllBodies.Count; i++)
            {
                if (AllBodies[i].GetBodyPhysics() != ThisBody && AllBodies[i].GetBodyObject().tag != "Weapon" && !AllBodies[i].GetBodyScript().IsCollided())
                {


                    float AccelerationMagnitude = (GravitationalConstant * AllBodies[i].GetBodyPhysics().mass) /
                        Mathf.Pow((Vector2.Distance(FuturePosition, AllBodies[i].GetBodyScript().GetFuturePosition())), 2f);
                    Vector2 AccelerationVector = AccelerationMagnitude * (AllBodies[i].GetBodyScript().GetFuturePosition() - FuturePosition).normalized;
                    ResultantAcceleration = ResultantAcceleration + AccelerationVector;
                }

            }

            FutureVelocity = FutureVelocity + ResultantAcceleration * Time.fixedDeltaTime;

        }

    }

    public void CalculateFuturePosition(GravitationalForce Subject)
    {
        if (!CollisionDetected)
        {

            FuturePosition = FuturePosition + FutureVelocity * Time.fixedDeltaTime;
            if (Subject != null)
            {
                FuturePosition = FuturePosition - Subject.GetFutureVelocity() * Time.fixedDeltaTime;
            }

            TrajectoryQueue.Enqueue(new TrajectoryProperties(FuturePosition, FutureVelocity));

        }


    }

    public void CalculateNewFutureVelocity(List<NewTrajectoryProperies> AllBodies)
    {


        if (!CollisionDetected)
        {
            Vector2 ResultantAcceleration = new Vector2();

            for (int i = 0; i < AllBodies.Count; i++)
            {



                float AccelerationMagnitude = (GravitationalConstant * AllBodies[i].getMass()) /
                    Mathf.Pow((Vector2.Distance(FuturePosition, AllBodies[i].getPosition())), 2f);
                Vector2 AccelerationVector = AccelerationMagnitude * (AllBodies[i].getPosition() - FuturePosition).normalized;
                ResultantAcceleration = ResultantAcceleration + AccelerationVector;


            }

            FutureVelocity = FutureVelocity + ResultantAcceleration * Time.fixedDeltaTime;
        }

    }

    public void CalculateNewFuturePosition(GravitationalForce Subject, int Index)
    {
        if (!CollisionDetected)
        {


            FuturePosition = FuturePosition + FutureVelocity * Time.fixedDeltaTime;
            if (Subject != null)
            {
                FuturePosition = FuturePosition - Subject.GetTrajectoryVelocityAtIndex(Index) * Time.fixedDeltaTime;
            }

            TrajectoryQueue.Enqueue(new TrajectoryProperties(FuturePosition, FutureVelocity));


        }
    }

    public ParticleSystem PS;
    private int Health;
    private bool Invincible;
    private int Damage;

    public void DamageBody(int IncomingDamage)
    {
        Health = Health - IncomingDamage;
        if (Health <= 0 && !Invincible)
        {
            CollisionDetected = true;
            // PredictionBody.GetComponent<PreviewProperties>().SetCollisionDetected(CollisionDetected);


        }
    }

    private bool CollisionDetected = false;

    public void SetCollisionPositions(int Index)
    {
        if (Index < 0)
        {
            PredictionBodyPhysics.position = FuturePosition;
        }
        else
        {
            PredictionBodyPhysics.position = TrajectoryQueue.ToArray()[Index].getPosition();
        }

        //  PredictionBodySprite.enabled = true;
    }


    public void CheckForCollisions()
    {
        if (!CollisionDetected)
        {
            List<Collider2D> Results = new List<Collider2D>();
            // Results.Clear();


            PredictionBodyPhysics.OverlapCollider(ColliderFilter, Results);

            if (Results.Count > 0)
            {
                foreach (Collider2D Col in Results)
                {

                    DamageBody(Col.gameObject.GetComponent<PreviewProperties>().GetDamage());
                }

                //  PredictionBodySprite.color = Color.red;

                // print(PredictionBodyPhysics.position);
            }
            else
            {
                // PredictionBodySprite.color = Color.blue;

            }

        }

        // print(Results.Count);
    }

    public void MoveIfCollided()
    {

        if (CollisionDetected)
        {

            PredictionBody.transform.position = new Vector3(PredictionBody.transform.position.x, PredictionBody.transform.position.y, 1.6f);

        }
    }

    private bool IsDestroyed;
    private TrajectoryProperties CurrentTrajectoryProperties;
    public void UpdatePosition()
    {



        if (TrajectoryQueue.Count == 0)
        {
            IsDestroyed = true;


        }
        else
        {
            CurrentTrajectoryProperties = TrajectoryQueue.Dequeue();


            ThisBody.position = CurrentTrajectoryProperties.getPosition();


            //   PredictionBodyPhysics.position = CurrentTrajectoryProperties.getPosition();

            if (ThisBody.tag == "Weapon")
            {
                //check if rag to degrees is needed (i.e. dont use euler)
                float angle = Mathf.Atan2(CurrentTrajectoryProperties.getVelocity().y, CurrentTrajectoryProperties.getVelocity().x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                //PredictionBody.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

    }


    public void DrawTrajectory(int Steps, bool Previewing)
    {

        TrajectoryProperties[] TrajectoryArray = TrajectoryQueue.ToArray();

        if (Steps > TrajectoryArray.Length)
        {
            Steps = TrajectoryArray.Length;
        }

        // print(gameObject.name + " " + Steps + " " + CollisionDetected + " " + PredictionBody.transform.position.z);


        PredictionBodySprite.enabled = true;
        if (Steps > 0)
        {
            TrajectoryProperties TrajectoryProperties = TrajectoryArray[Steps - 1];

            if (Previewing)
            {

                PredictionBody.transform.position = new Vector3(TrajectoryProperties.getPosition().x, TrajectoryProperties.getPosition().y, PredictionBody.transform.position.z);

            }
            else
            {
                PredictionBodyPhysics.position = TrajectoryProperties.getPosition();

            }

            if (ThisBody.CompareTag("Weapon"))
            {
                //check if rag to degrees is needed (i.e. dont use euler)
                float angle = Mathf.Atan2(TrajectoryProperties.getVelocity().y, TrajectoryProperties.getVelocity().x) * Mathf.Rad2Deg;
                PredictionBody.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }

        LR.positionCount = Steps + 1;
        LR.SetPosition(0, new Vector3(ThisBody.position.x, ThisBody.position.y, 5));

        for (int i = 0; i < Steps; i++)
        {
            LR.SetPosition(i + 1, new Vector3(TrajectoryArray[i].getPosition().x, TrajectoryArray[i].getPosition().y, 5));

        }

        //if (gameObject.tag == "Weapon")
        //{
        //  //  PredictionBodyPhysics.position = TrajectoryArray[Steps - 1].getPosition();
        //  //  Debug.Log(PredictionBodyPhysics.position);
        //}



    }

    public void ResetPrediction()
    {

        PredictionBodySprite.enabled = false;
        LR.positionCount = 0;
    }
    public void ResetPreview()
    {

        PredictionBody.transform.position = new Vector3(ThisBody.transform.position.x, ThisBody.transform.position.y, 1);
        PredictionBody.GetComponent<PreviewProperties>().SetDamage(Damage);
    }
    public void RemoveLine()
    {
        Destroy(LR.gameObject);
    }

    public void RemovePredictionBody()
    {

        Destroy(PredictionBody);
    }

    public Vector2 GetFutureVelocity()
    {
        return FutureVelocity;
    }


    public Vector2 GetFuturePosition()
    {
        return FuturePosition;
    }

    public Vector2 GetCurrentVelocity()
    {
        return CurrentTrajectoryProperties.getVelocity();
    }

    public bool IsCollided()
    {
        return CollisionDetected;
    }



    public NewTrajectoryProperies GetTrajectoryPropertiesAtIndex(int Index)
    {
        TrajectoryProperties[] TrajectoryArray = TrajectoryQueue.ToArray();
        NewTrajectoryProperies ReturnedTrajectoryProperies = new NewTrajectoryProperies(TrajectoryArray[Index].getPosition(), ThisBody.mass);

        return ReturnedTrajectoryProperies;
    }

    public Vector2 GetTrajectoryVelocityAtIndex(int Index)
    {
        return TrajectoryQueue.ToArray()[Index].getVelocity();
    }

    public bool IsCollidedAtIndex(int Index)
    {
        return Index >= TrajectoryQueue.Count;
    }

    public bool RemoveIfDestroyed()
    {
        if (IsDestroyed)
        {


            if (gameObject.CompareTag("Body"))
            {

                var PSSize = PS.main;
                PSSize.startSize = gameObject.GetComponent<MassCalculator>().Radius;
                ParticleSystem ps = Instantiate(PS, gameObject.transform.position, Quaternion.identity);
                BodiesController.RemoveBody(gameObject);
                Destroy(gameObject);
                Destroy(ps.gameObject, 1f);
            }
            else

            {
                var PSSize = PS.main;
                PSSize.startSize = 4;


                ParticleSystem ps = Instantiate(PS, gameObject.transform.position, Quaternion.identity);
                BodiesController.RemoveBody(gameObject);
                Destroy(gameObject);
                Destroy(ps.gameObject, 1f);

            }
        }

        return IsDestroyed;

    }

}
