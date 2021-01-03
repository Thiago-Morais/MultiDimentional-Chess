using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests_PlayMode
{
    public class SceneTests
    {
        [UnityTest]
        public IEnumerator LoadScene_SceneBuildIndex_SetActiveSceneToSceneAtBuildIndex([Values(0, 1, 2)] int buildIndex)
        {
            //SETUP
            SceneLoader sceneLoader = new GameObject().AddComponent<SceneLoader>();
            //ACT
            sceneLoader.LoadScene(buildIndex);
            yield return null;
            //ASSERT
            Assert.True(SceneManager.GetSceneByBuildIndex(buildIndex).isLoaded);
        }
    }
}