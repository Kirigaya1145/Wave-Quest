using UnityEngine;

public class WaveAnimator : MonoBehaviour
{
    public LineRenderer line;
    public bool isTransversal = true;

    public int pointCount = 50;
    public float panjang = 20f;
    public float amplitudo = 1f;
    public float kecepatan = 2f;

    public float coilRadius = 0.2f;
    public int coilSegments = 12;
    public float kerapatan = 0.6f;
    private LineRenderer[] coils;

    private float time;
    
    public bool isDashed = false;
    [SerializeField] private int dashCount = 15;
    private LineRenderer[] dashes;

    public bool usePanjangGelombang = false;
    public float panjangGelombang = 4f;
    float K => usePanjangGelombang ? 2f * Mathf.PI / panjangGelombang : kerapatan;
    void Start()
    {
        coils = new LineRenderer[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            GameObject coilObj = new GameObject("Coil" + i);
            coilObj.transform.parent = transform;

            LineRenderer lineR = coilObj.AddComponent<LineRenderer>();
            lineR.positionCount = coilSegments;
            lineR.loop = true; 
            lineR.startWidth = 0.05f;
            lineR.endWidth = 0.05f;
            lineR.material = line.material;
            lineR.sortingLayerID = line.sortingLayerID;
            lineR.sortingOrder = line.sortingOrder;

            coils[i] = lineR;
        }

        dashes = new LineRenderer[dashCount];
        for (int i = 0; i < dashCount; i++)
        {
            GameObject dashObject = new GameObject("Dash" + i);
            dashObject.transform.parent = transform;
            LineRenderer lr = dashObject.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.material = line.material;
            lr.sortingLayerID = line.sortingLayerID;
            lr.sortingOrder = line.sortingOrder;
            dashes[i] = lr;
        }
    }

    void Update()
    {
        time += Time.deltaTime * kecepatan;
        foreach (var c in coils) c.gameObject.SetActive(!isTransversal);
        if (isTransversal)
        {
            if (isDashed)
            {
                line.enabled = false;
                foreach (var d in dashes) d.gameObject.SetActive(true);
                DrawDashedWave();
            }
            else
            {
                line.enabled = true;
                foreach (var d in dashes) d.gameObject.SetActive(false);

                line.positionCount = pointCount;
                for (int i = 0; i < pointCount; i++)
                {
                    float t = i / (float)pointCount;
                    float x = t * panjang - (panjang / 2f);
                    float y = amplitudo * Mathf.Sin(x * K - time);
                    line.SetPosition(i, new Vector3(x, y, 0));
                }
            }
        }
        else
        {
            line.enabled = false;
            foreach (var d in dashes) d.gameObject.SetActive(false);
            for (int i = 0; i < pointCount; i++)
            {
                float t = i / (float)pointCount;
                float baseX = t * panjang - (panjang / 2f);
                float offset = amplitudo * Mathf.Sin(baseX * K - time);
                float centerX = baseX + offset;

                for (int j = 0; j < coilSegments; j++)
                {
                    float angle = (j / (float)coilSegments) * Mathf.PI * 2f;
                    float px = centerX + Mathf.Cos(angle) * coilRadius * 0.15f;
                    float py = Mathf.Sin(angle) * coilRadius;
                    coils[i].SetPosition(j, new Vector3(px, py, 0));
                }
            }
        }
    }
    void DrawDashedWave()
    {
        for (int i = 0; i < dashCount; i++)
        {
            float t1 = i / (float)dashCount;
            float t2 = t1 + (0.5f / dashCount);
            float x1 = t1 * panjang - (panjang / 2f);
            float y1 = amplitudo * Mathf.Sin(x1 * K - time);
            float x2 = t2 * panjang - (panjang / 2f);
            float y2 = amplitudo * Mathf.Sin(x2 * K - time);

            dashes[i].SetPosition(0, new Vector3(x1, y1, 0));
            dashes[i].SetPosition(1, new Vector3(x2, y2, 0));
        }
    }
}
