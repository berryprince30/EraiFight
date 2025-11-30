using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
namespace PlayerOwnedStates
{
    public class Idle : State<Player>
    {
        public override void Enter(Player player)
        {
            
        }
        public override void Execute(Player player)
        {
            // player.P_anim.SetBool("Idle", true);
        }
        public override void Exit(Player player)
        {
            // player.P_anim.SetBool("Idle", false);
        }
    }
    public class Moving : State<Player>
    {
        public override void Enter(Player player)
        {

        }
        public override void Execute(Player player)
        {
            Debug.Log("aa");
            player.P_anim.SetBool("Walk", true);
        }
        public override void Exit(Player player)
        {
            player.P_anim.SetBool("Walk", false);
        }
    }
    public class Jumping : State<Player>
    {
        public override void Enter(Player player)
        {

        }
        public override void Execute(Player player)
        {
            player.P_anim.SetBool("Jump", true);
        }
        public override void Exit(Player player)
        {
            player.P_anim.SetBool("Jump", false);
        }
    }
    public class Attacking : State<Player>
    {
        public override void Enter(Player player)
        {

        }
        public override void Execute(Player player)
        {

        }
        public override void Exit(Player player)
        {

        }
    }
    public class Sstun : State<Player>
    {
        public override void Enter(Player player)
        {
            player.photonView.RPC("checkUI", RpcTarget.AllBuffered, player.CurHP, player.MaxHP);
        }
        public override void Execute(Player player)
        {

        }
        public override void Exit(Player player)
        {

        }
    }
    public class Lstun : State<Player>
    {
        public override void Enter(Player player)
        {
            player.photonView.RPC("checkUI", RpcTarget.AllBuffered, player.CurHP, player.MaxHP);
        }
        public override void Execute(Player player)
        {

        }
        public override void Exit(Player player)
        {

        }
    }
    public class Die : State<Player>
    {
        public override void Enter(Player player)
        {
            // player.P_anim.SetTrigger("Die");
        }
        public override void Execute(Player player)
        {

        }
        public override void Exit(Player player)
        {

        }
    }
    public class Guard : State<Player>
    {
        public override void Enter(Player player)
        {
            // player.P_anim.SetTrigger("Guard");
        }
        public override void Execute(Player player)
        {

        }
        public override void Exit(Player player)
        {

        }
    }
    public class Special : State<Player>
    {
        public override void Enter(Player player)
        {

        }
        public override void Execute(Player player)
        {

        }
        public override void Exit(Player player)
        {

        }
    }
}