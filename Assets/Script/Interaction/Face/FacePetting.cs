using UnityEngine;

public class FacePetDetector : MonoBehaviour
{
    private Animator targetAnim;
    private bool canPet = false;

    public void Init(Animator anim)
    {
        targetAnim = anim;
    }

    public void EnablePetting(bool enable)
    {
        canPet = enable;
    }

    void Update()
    {
        if (!canPet) return;
        if (targetAnim == null) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Debug.Log($"[PET] touch phase = {touch.phase}");  

            if (touch.phase == TouchPhase.Moved)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.root == transform.root)
                    {
                        targetAnim.SetBool("IsPetting", true);
                        bool currentVal = targetAnim.GetBool("IsPetting");
                        Debug.Log($"[PET] Set IsPetting = true, current value = {currentVal}");
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                targetAnim.SetBool("IsPetting", false);
                Debug.Log("[PET] Petting END");
            }
        }
    }
}