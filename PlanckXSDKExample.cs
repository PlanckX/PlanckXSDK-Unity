using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Planckx.Sdk.Client;
using Planckx.Sdk.Bean;
using static Planckx.Sdk.Bean.ResponseCode;

public class PlanckXSDKExample : MonoBehaviour {
    public UnityEngine.UI.Text txtBindStatus = default;
    private PlanckxAccountClient accountClient = default;
    public UnityEngine.UI.Button btnBindButton = default;
    public UnityEngine.UI.Text txtGetAllGameNFTTestCase = default;
    public UnityEngine.UI.Text txtGetAllPlayerNFTTestCase = default;
    public UnityEngine.UI.Text txtGetNFTByTokenIdTestCase = default;
    private const string PlanckXAPIKey = "";
    private const string PlanckXSecretKey = "";
    private string PlayerId;
    void Start() {
        PlayerId = SystemInfo.deviceUniqueIdentifier;
        accountClient = Planckx.PlanckxClient.AccountClient(PlanckXAPIKey, PlanckXSecretKey);
    }
    public async void OnBtnCbBind() {
        if (string.IsNullOrEmpty(PlanckXAPIKey) || string.IsNullOrEmpty(PlanckXSecretKey)) {
            Debug.LogError("API Key or SecretKey is null");
            return;
        }
        btnBindButton.interactable = false;
        var res = await accountClient.BindState(PlayerId);
        if (res.Item1 == ResponseEnum.Successful) {
            if (res.Item2.Bind) {
                btnBindButton.interactable = true;
                txtBindStatus.text = "Bind";
                txtBindStatus.color = Color.green;
            } else {
                //如果没有绑定，那么res.Item2.AuthAddress里是绑定的地址，可以用浏览器或者webview进行打开
                Application.OpenURL(res.Item2.AuthAddress);
                StartCoroutine(CheckBind());
            }
        }
    }
    private async void DoCheckBind() {
        var res = await accountClient.BindState(PlayerId);
        if (res.Item1 == ResponseEnum.Successful) {
            if (res.Item2.Bind) {
                txtBindStatus.text = "Bind";
                txtBindStatus.color = Color.green;
                btnBindButton.interactable = true;
                StopAllCoroutines();
            }
        }
    }
    IEnumerator CheckBind() {
        while (true) {
            //每1.5秒检查绑定状态
            DoCheckBind();
            yield return new WaitForSeconds(1.5f);
        }
    }
    //
    //
    //
    //
    public async void OnBtnCbGetAllGameNFTsTest() {
        if (string.IsNullOrEmpty(PlanckXAPIKey) || string.IsNullOrEmpty(PlanckXSecretKey)) {
            Debug.LogError("API Key or SecretKey is null");
            return;
        }
        PlanckxNftClient nftClient = Planckx.PlanckxClient.NftClient(PlanckXAPIKey, PlanckXSecretKey);
        var res = await nftClient.AllNfts();
        if (res.Item1 == ResponseEnum.Successful) {
            txtGetAllGameNFTTestCase.color = Color.green;
            txtGetAllGameNFTTestCase.text = "Query Succeed";
        } else {
            txtGetAllGameNFTTestCase.color = Color.red;
            txtGetAllGameNFTTestCase.text = "Query Failed";
        }
    }
    //
    //
    //
    //
    public async void OnBtnCbGetAllPlayerNFTsTest() {
        if (string.IsNullOrEmpty(PlanckXAPIKey) || string.IsNullOrEmpty(PlanckXSecretKey)) {
            Debug.LogError("API Key or SecretKey is null");
            return;
        }
        PlanckxNftClient nftClient = Planckx.PlanckxClient.NftClient(PlanckXAPIKey, PlanckXSecretKey);
        var res = await nftClient.NftByPlayer(PlayerId);
        if (res.Item1 == ResponseEnum.Successful) {
            txtGetAllPlayerNFTTestCase.color = Color.green;
            txtGetAllPlayerNFTTestCase.text = "Query Succeed";
        } else {
            txtGetAllPlayerNFTTestCase.color = Color.red;
            txtGetAllPlayerNFTTestCase.text = "Query Failed";
        }
    }
    //
    //
    //
    //
    public async void OnBtnCbGetNFTByTokenIdTest() {
        if (string.IsNullOrEmpty(PlanckXAPIKey) || string.IsNullOrEmpty(PlanckXSecretKey)) {
            Debug.LogError("API Key or SecretKey is null");
            return;
        }
        PlanckxNftClient nftClient = Planckx.PlanckxClient.NftClient(PlanckXAPIKey, PlanckXSecretKey);
        var res = await nftClient.NftByTokenId("tokenId");
        if (res.Item1 == ResponseEnum.Successful) {
            txtGetNFTByTokenIdTestCase.color = Color.green;
            txtGetNFTByTokenIdTestCase.text = "Query Succeed";
        } else {
            txtGetNFTByTokenIdTestCase.color = Color.red;
            txtGetNFTByTokenIdTestCase.text = "Query Failed";
        }
    }
}
