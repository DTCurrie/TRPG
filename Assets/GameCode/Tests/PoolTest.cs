using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolTest : MonoBehaviour
{
    public const int ObjCount = 100;
    public const int TestCount = 1000;

    public PoolTestPooler PoolTestSpheres;
    public PoolTestPooler PoolTestCubes;

    private void Start()
    {

        if (PoolController.AddPool(PoolTestSpheres.Index, PoolTestSpheres.Prefab, ObjCount, 0))
            UnityEngine.Debug.Log("Pre-populating sphere pool");
        else
            UnityEngine.Debug.Log("Sphere pool already configured");

        if (PoolController.AddPool(PoolTestCubes.Index, PoolTestCubes.Prefab, ObjCount, 0))
            UnityEngine.Debug.Log("Pre-populating cube pool");
        else
            UnityEngine.Debug.Log("Cube pool already configured");
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "Scenes");
        if (GUI.Button(new Rect(120, 10, 100, 30), "Test Pools")) StartCoroutine(TestPools());
        if (GUI.Button(new Rect(10, 50, 100, 30), "Scene 1")) ChangeLevel(0);
        if (GUI.Button(new Rect(120, 50, 100, 30), "Scene 2")) ChangeLevel(1);
    }

    private void TestPool(PoolTestPooler pooler)
    {
        var watch = new Stopwatch();
        watch.Start();

        for (int i = 0; i < TestCount; i++)
        {
            for (int j = 0; j < ObjCount; j++)
            {
                pooler.Instances.Add(PoolController.Dequeue(pooler.Index));
            }

            for (int j = ObjCount - 1; j >= 0; j--)
            {
                PoolController.Enqueue(pooler.Instances[j]);
                pooler.Instances.RemoveAt(j);
            }
        }


        watch.Stop();
        UnityEngine.Debug.Log(string.Format("Completed {0} in {1} ms", pooler.Prefab.name, watch.Elapsed.Milliseconds));
    }

    private IEnumerator TestPools()
    {
        TestPool(PoolTestSpheres);
        yield return new WaitForSeconds(1);
        TestPool(PoolTestCubes);
    }

    private void ChangeLevel(int level)
    {
        ReleaseInstances();
        SceneManager.LoadScene("PoolTest" + level);
    }

    private void ReleaseInstances()
    {
        for (int i = PoolTestSpheres.Instances.Count - 1; i >= 0; i--)
            PoolController.Enqueue(PoolTestSpheres.Instances[i]);

        PoolTestSpheres.Instances.Clear();

        for (int i = PoolTestCubes.Instances.Count - 1; i >= 0; i--)
            PoolController.Enqueue(PoolTestCubes.Instances[i]);

        PoolTestCubes.Instances.Clear();
    }
}
