using UnityEngine;

public class Follow_player : MonoBehaviour {

    public Transform player;
    public Vector2 turn;
    public float sensitivity = 0.5f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update () {
//        transform.position = player.transform.position + new Vector3(0, 1, -1);

        turn.x += Input.GetAxis ("Mouse X") * sensitivity;
		turn.y += Input.GetAxis ("Mouse Y") * sensitivity;
		transform.localRotation = Quaternion.Euler (-turn.y, turn.x, 0);
    }
}
