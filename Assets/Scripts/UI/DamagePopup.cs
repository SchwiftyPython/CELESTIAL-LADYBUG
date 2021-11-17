using System.Collections;
using Assets.Scripts.Combat;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class DamagePopup : MonoBehaviour
    {

        public static void Create(Vector3 position, int damageAmount, bool isCriticalHit)
        {
            var combatManager = FindObjectOfType<CombatManager>();
            
            GameObject damagePopupTransform = Instantiate(combatManager.DamagePopupPrefab, new Vector3(position.x, position.y + .7f), Quaternion.identity);
            
            DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
            damagePopup.Setup(damageAmount, isCriticalHit);
        }

        private static int sortingOrder;

        private const float DISAPPEAR_TIMER_MAX = 1f;

        private TextMeshPro textMesh;
        private float disappearTimer;
        private Color textColor;
        private Vector3 moveVector;

        private void Awake()
        {
            textMesh = transform.GetComponent<TextMeshPro>();
        }

        public void Setup(int damageAmount, bool isCriticalHit)
        {
            if (damageAmount < 0)
            {
                damageAmount = 0;
            }

            textMesh.SetText(damageAmount.ToString());
            if (!isCriticalHit)
            {
                // Normal hit
                textMesh.fontSize = 7.7f;
                textColor = GlobalHelper.GetColorFromString("fee761");
            }
            else
            {
                // Critical hit
                textMesh.fontSize = 8.3f;
                textColor = GlobalHelper.GetColorFromString("ff0044");
            }
            textMesh.color = textColor;
            disappearTimer = DISAPPEAR_TIMER_MAX;

            sortingOrder++;
            textMesh.sortingOrder = sortingOrder;

            StartCoroutine(AnimateText(isCriticalHit));
        }

        public IEnumerator AnimateText(bool isCriticalHit)
        {
            textMesh.transform.DOMoveY(transform.position.y + 1, 1f);

            if (isCriticalHit)
            {
                yield return textMesh.DOScale(2f, .2f).WaitForCompletion();
            }
            else
            {
                yield return textMesh.DOScale(1.2f, .2f).WaitForCompletion();
            }

            textMesh.DOScale(.8f, .8f);

            yield return textMesh.DOFade(0, 1.5f).WaitForCompletion();

            Destroy(gameObject);
        }
    }
}
