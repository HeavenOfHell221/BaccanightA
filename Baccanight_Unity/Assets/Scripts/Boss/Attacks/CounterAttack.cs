using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttack : BossAttack
{
    #region Inspector
    [Header("General")]
    [Space(5)]
    [SerializeField] private Collider2D m_collider;
    //[SerializeField] private Collider2D m_hitCollider;
    //[SerializeField] private LayerMask m_layerCollision;
    //[SerializeField] private LayerMask m_layerHit;
    [SerializeField] private Rigidbody2D m_rigidbody;
    //[SerializeField] private MinMaxFloat m_distanceToPlayer;
    [SerializeField] private HealthBoss m_health;

    [Header("Phase 1")]
    [Space(5)]
    [SerializeField] [Range(1, 30)] private int m_speed = 10;
    //[SerializeField] [Range(1, 3)] private int m_hitDamage = -1;
    [SerializeField] [Range(0.1f, 3f)] private float m_cooldownBeforeAttack = 1.5f;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(1, 30)] private int m_speedUpgrade = 20;
    [SerializeField] [Range(1, 3)] private int m_hitDamageUpgrade = -1;
    [SerializeField] [Range(0.1f, 3f)] private float m_cooldownBeforeAttackUpgrade = 1.0f;
    #endregion

    private Transform m_player;

    private void Start()
    {
        m_collider.enabled = false; // Pour pas trigger les évents
                                    // on active le collider seulement pendant l'attaque.

        // Dans le start, comme ça tu le refait pas à chaque fois que tu appels Move(). :p
        // C'est la référence du transform du joueur. Si tu veux la position à un instant T du joueur il faut
        // prendre transform.position, qui retourne un vector3.
        m_player = PlayerManager.Instance.PlayerReference.transform;
    }

    [ContextMenu("Start Attack")]
    public override void StartAttack()
    {
        base.StartAttack();
        StartCoroutine(HandleAttack());
    }

    /* Dans l'inspecteur, tu as mis les layers "Player" et "Stage" dans l'EventScriptTrigger.
     * Du coup, quand le collider va entrer en collision avec le player il va être trigger et faire 
     * l'évent (qui appelle cette méthode). MAIS, il va être trigger aussi quand tu entre dans un objet ayant pour layer "Stage".
     * Ca marche mais il faut faire des tests avec le gameobject other pour savoir si c'est le stage ou le player etc..
     * 
    public void OnEnterDetect(GameObject other)
    {
        other.GetComponent<Health>().ModifyHealth(m_hitDamage, gameObject);
    }
    */

    /*
     * A la place j'ai mis 2 EventScriptTrigger. L'un va Invoke un event quand il trigger avec le player.
     * L'autre quand il trigger avec le stage. Comme ça, tu peux séparer ce que tu as à faire! :D
     */

    // Quand le collider entre dans le stage
    // On arrête l'attaque. Dans EndAttack il y a la désactivation du collier et la vitesse du rigibody à zero.
    public void OnEnterStage()
    {
        /*
         * A la place de ton While! Il fonctionnait très bien mais là je prend 1 ligne de code alors
         * qu'avec le while c'était un if à chaque frame
         */        
        EndAttack();       
    }

    // Quand le collider entre dans un objet ayant pour layer "Player". Je précise ça car c'est pas forcément le joueur..
    // Dans notre cas si car j'ai mis ce layer qu'a un seul gameobject mais c'est un détail à pas oublier :D
    // C'est pour ça que j'appelle pas le GameObject en paramètre "player".
    /*public void OnEnterPlayer(GameObject other)
    {
        other.GetComponent<Health>().ModifyHealth(m_hitDamage, gameObject);       
    }*/

    protected override IEnumerator HandleAttack()
    {
        /*
         * Pas besoin de mettre le WaitForSeconds à 0 si on cancel l'attaque.
         * Dans le méthode CancelAttack() il y a StopAllCoroutines() qui va stopper HandleAttack (en interne je ne sais pas comment c'est
         * fait mais il me semble qu'il l'arrête brutalement sans que la coroutine finisse, à voir si c'est vrai).
         */
        yield return new WaitForSeconds(m_cooldownBeforeAttack);

        m_health.IsInvincible = true; // Pour rendre la contre-attaque plus drole, le joueur peut pas lui faire de dégâts ? 
                                      // A voir, tu peux retirer si tu trouves ça trop.

        StartCoroutine(MoveTowardPlayer());

        /*while (!m_collider.IsTouchingLayers(m_layerCollision))
        {
            yield return null;
        }

        m_rigidbody.velocity = Vector2.zero;

        EndAttack();*/
    }

    private IEnumerator MoveTowardPlayer()
    {    
        Vector3 direction = (m_player.position - transform.position).normalized;
        //m_rigidbody.AddForce(direction * m_speed, ForceMode2D.Impulse);
        m_rigidbody.velocity = direction * m_speed;

        /* Soit tu mets une force, soit tu modifies sa vélocité. 
         * Ajouter une force c'est utiliser la physique de Unity pour qu'en interne il calcule la vélocité de l'objet.
         * C'est comme.. Pousser un objet dans une direction avec une certaine force (la velocité est caculé en interne suivant les 
         * paramètre du rigidbody tel que la gravité, sa masse, la friction du sol etc.).
         * 
         * "m_rigidbody.velocity = ..." C'est attribué toi-même une vélocité à l'objet.
         * 
         * c'est comme si tu avais fait :
         * (1) int a = 2;
         * (2) a = DoublerValeur(a); -> a = 4
         * (3) a = 896;
         * (4) debug.log(a); -> a = 896
         * 
         * La ligne (2) sert à rien. Ton AddForce aussi. x)
         */

        yield return new WaitForSeconds(0.1f);
        m_collider.enabled = true; // On active le Collider pour detecter le joueur et le stage.
                                   // On l'active après 0.1 seconde pour pas que le collider detecte le bord du stage
                                   // qui l'a arrêté à sa dernière contre-attaque..
    }

    [ContextMenu("End Attack")]
    protected override void EndAttack()
    {
        ResetAttributes();
        base.EndAttack();
    }

    [ContextMenu("Upgrade Attack")]
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        m_speed = m_speedUpgrade;
        m_cooldownBeforeAttack = m_cooldownBeforeAttackUpgrade;
        //m_hitDamage = m_hitDamageUpgrade;
    }

    [ContextMenu("Cancel Attack")]
    public override void CancelAttack()
    {
        StopAllCoroutines();
        ResetAttributes();
        base.CancelAttack();
    }

    private void ResetAttributes()
    {
        m_rigidbody.velocity = Vector2.zero;
        m_collider.enabled = false;
        m_health.IsInvincible = false;
    }
}
