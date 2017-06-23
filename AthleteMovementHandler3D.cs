
using System.Collections.Generic;
using UnityEngine;

namespace Fiziks3D
{
    public class AthleteMovementHandler3D : MonoBehaviour
    {
        private List<IMovementCalculator> calculators;
        public Vector3 TotalMovement { get; private set; }
        private CharacterController character;

        private void Start()
        {
            character = GetComponent<CharacterController>();

            // 移動量を計算するクラスをそれぞれここで登録する。
            calculators = new List<IMovementCalculator>();
            calculators.Add(new WalkCalculator3D(this.gameObject));
            calculators.Add(new GravityCalculator3D(this.gameObject));
            calculators.Add(new JumpCalculator3D(this.gameObject));
            calculators.Add(new InertiaCalculator3D(this.gameObject));
        }


        private void Update()
        {
            TotalMovement = Vector3.zero;

            // 移動量を計算するクラスをそれぞれ、Update して合計する。毎フレーム。
            foreach(IMovementCalculator element in calculators)
            {
                element.ManualUpdate();
                TotalMovement += element.MovementPerFrame;
            }

            // ワールド座標で移動する。Translate では CharacterController が傾斜に沿って歩いてくれない。
            //transform.Translate(TotalMovement * Time.deltaTime, Space.World);
            character.Move(TotalMovement * Time.deltaTime);
        }
    }


    /// <summary>
    /// 移動量を計算するクラスのためのインターフェイス。
    /// </summary>
    public interface IMovementCalculator
    {
        /// <summary>
        /// MovementHandler でこの値を合計する。
        /// </summary>
       Vector3 MovementPerFrame { get; }

        /// <summary>
        /// MovementHangler で毎フレーム呼び出す。
        /// </summary>
        void ManualUpdate();
    }
}