using UnityEngine;

public class Star : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Periksa apakah objek yang menabrak memiliki tag "Player"
        if (other.CompareTag("unit"))
        {
            // Panggil metode untuk menambah skor
            ScoreManager.Instance.IncrementScore();

            // Nonaktifkan bintang (atau hapus dari permainan)
            gameObject.SetActive(false);
        }
    }
}
