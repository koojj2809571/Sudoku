using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

// ReSharper disable once CheckNamespace
namespace UI
{
    public class StartScreenCtr : MonoBehaviour
    {

        public LevelDialogCtr levelCtr;
        public Text finishLevelText;

        private void Start()
        {
            var levelCount = LevelRunData.Instance.GameResult.Count;
            if (levelCount != 0)
            {
                finishLevelText.text = $"{levelCount} / 100";
            }
        }

        public void ClickRandomGame()
        {
            LevelRunData.Instance.diffLevel = 45;
            LevelRunData.Instance.SelectedGameIndex = -1;
            var seed = DateTimeOffset.Now.ToUnixTimeSeconds().ToString().GetHashCode();
            LevelRunData.Instance.RanSeed = seed;
            var ran = new System.Random(seed);
            var gameIndex = ran.Next(81);
            var gameTransformer = new Matrix9X9(LevelRunData.Instance.GameSeed[gameIndex]);
            gameTransformer.RandomTransform(ran, 3);
            LevelRunData.Instance.RandomGame = gameTransformer.Src;
            SceneManager.LoadScene(1);
        }

        public void ClickDailyChallenge(int level)
        {
            LevelRunData.Instance.diffLevel = level;
            LevelRunData.Instance.SelectedGameIndex = -1;
            var now = DateTime.Now;
            var seed = $"{now.Year}-{now.Month}-{now.Day}".GetHashCode();
            LevelRunData.Instance.RanSeed = seed;
            var ran = new System.Random(seed);
            var gameIndex = ran.Next(81);
            LogUtil.Log(gameIndex.ToString());
            LogUtil.Log(LevelRunData.Instance.GameSeed[gameIndex].LogStr());
            
            var gameTransformer = new Matrix9X9(LevelRunData.Instance.GameSeed[gameIndex]);
            LogUtil.Log(gameTransformer.ToString());
            // gameTransformer.SwapCol(ran);
            // gameTransformer.SwapRow(ran);
            // gameTransformer.SwapNumber(ran);
            LevelRunData.Instance.RandomGame = gameTransformer.Src;
            SceneManager.LoadScene(1);
        }

        public void ClickLevelSelectBt()
        {
            levelCtr.gameObject.SetActive(true);
            StartCoroutine(OpenDialog());
        }

        private IEnumerator OpenDialog()
        {
            yield return new WaitForSeconds(0.3f);
            levelCtr.DialogSwitcher(true);
        }

        public void CloseDialog()
        {
            levelCtr.DialogSwitcher(false);
        }

        public void Test()
        {
            var matrix9X9 = new Matrix9X9(LevelRunData.Instance.GameSeed[0]);
            LogUtil.Log(matrix9X9.ToString());
            // matrix9X9.Transpose();
            // matrix9X9.SwapRow(0,2);
            // matrix9X9.SwapCol(0,2);
            // matrix9X9.SwapBigCol(0,2);
            // matrix9X9.SwapBigRow(0,2);
            matrix9X9.SwapNumber(1,9);
            LogUtil.Log(matrix9X9.ToString());
        }
    }
}
