using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImageAnim : MonoBehaviour
{
    public GameObject ImagePrefab;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        string[] images = { "권오태1", "권오태2", "김윤진1", "김윤진2", "변정민1", "변정민2", "임종운1", "임종운2", "조성민1", "조성민2" };

        images = images.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

        for (int i = 0; i < 10; i++)
        {
            GameObject newImg = Instantiate(ImagePrefab, new Vector3(0f + (i * 6f), -0.5f, 0f), Quaternion.identity);
            newImg.transform.parent = GameObject.Find("ImageWithAnim").transform;

            string imageName = images[i].ToString();
            print(imageName);
            newImg.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(imageName);

            newImg.transform.position += Vector3.forward * Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        //speed는 이미지 이동 속도(기본값 = 0.05f)
        transform.position += new Vector3(-speed, 0f, 0f);
    }

    void Update()
    {
        //이미지가 화면 밖으로 넘어가면 반복되도록
        if (transform.position.x < -60f)
        {
            transform.position = new Vector3(5.56f, 0f, 0f);
        }
    }
}
