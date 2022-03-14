using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlanckX;
public class PlanckXSDKExample : MonoBehaviour {
    public UnityEngine.UI.Text txtBindStatus = default;
    [SerializeField] private PlanckXClient plxClient = default;
    public UnityEngine.UI.Button btnBindButton = default;
    public UnityEngine.UI.Text txtGetAllGameNFTTestCase = default;
    public UnityEngine.UI.Text txtGetAllPlayerNFTTestCase = default;
    public UnityEngine.UI.Text txtGetNFTByTokenIdTestCase = default;
    private const string PlanckXAPIKey = "";
    private const string PlanckXSecretKey = "";
    private string PlayerId;
    void Start() {
        PlayerId = SystemInfo.deviceUniqueIdentifier;
        plxClient.SetAPIKeyAndSecretKey(PlanckXAPIKey, PlanckXSecretKey);
    }
    public void OnBtnCbBind() {
        StopAllCoroutines();
        if (string.IsNullOrEmpty(PlanckXAPIKey) || string.IsNullOrEmpty(PlanckXSecretKey)) {
            Debug.LogError("API Key or SecretKey is null");
            return;
        }
        btnBindButton.interactable = false;
        plxClient.CheckBindAsync(PlayerId, (isBind, bindURL) => {
            if (isBind) {
                btnBindButton.interactable = true;
                txtBindStatus.text = "Bind";
                txtBindStatus.color = Color.green;
            } else {
                //如果没有绑定，那么bindURL是绑定地址，可以用浏览器或者webview进行打开
                Application.OpenURL(bindURL);
                StartCoroutine(CheckBind());
            }
        }, (req) => {
            Debug.Log("Network Error");
        });
    }
    private void DoCheckBind() {
        plxClient.CheckBindAsync(PlayerId, (isBind, bindURL) => {
            if (isBind) {
                txtBindStatus.text = "Bind";
                txtBindStatus.color = Color.green;
                btnBindButton.interactable = true;
                StopAllCoroutines();
            }
        });
    }
    IEnumerator CheckBind() {
        while (true) {
            DoCheckBind();
            yield return new WaitForSeconds(1.5f);
        }
    }
    //
    //
    //
    //
    public void OnBtnCbGetAllGameNFTsTest() {
        StopAllCoroutines();
        if (string.IsNullOrEmpty(PlanckXAPIKey) || string.IsNullOrEmpty(PlanckXSecretKey)) {
            Debug.LogError("API Key or SecretKey is null");
            return;
        }
        plxClient.GetAllGameNFTAsync((nfts) => {
            txtGetAllGameNFTTestCase.color = Color.green;
            txtGetAllGameNFTTestCase.text = "Query Succeed";
        }, (req) => {
            txtGetAllGameNFTTestCase.color = Color.red;
            txtGetAllGameNFTTestCase.text = "Query Failed";
        });
    }
    //
    //
    //
    //
    public void OnBtnCbGetAllPlayerNFTsTest() {
        StopAllCoroutines();
        if (string.IsNullOrEmpty(PlanckXAPIKey) || string.IsNullOrEmpty(PlanckXSecretKey)) {
            Debug.LogError("API Key or SecretKey is null");
            return;
        }
        plxClient.GetNFTByPlayerIdAsync(PlayerId, (nfts) => {
            txtGetAllPlayerNFTTestCase.color = Color.green;
            txtGetAllPlayerNFTTestCase.text = "Query Succeed";
        }, (req) => {
            txtGetAllPlayerNFTTestCase.color = Color.red;
            txtGetAllPlayerNFTTestCase.text = "Query Failed";
        });
    }
    //
    //
    //
    //
    public void OnBtnCbGetNFTByTokenIdTest() {
        StopAllCoroutines();
        if (string.IsNullOrEmpty(PlanckXAPIKey) || string.IsNullOrEmpty(PlanckXSecretKey)) {
            Debug.LogError("API Key or SecretKey is null");
            return;
        }
        plxClient.GetNFTByTokenIdAsync("SomeTokenId", (nft) => {
            txtGetNFTByTokenIdTestCase.color = Color.green;
            txtGetNFTByTokenIdTestCase.text = "Query Succeed";
        }, (req) => {
            txtGetNFTByTokenIdTestCase.color = Color.red;
            txtGetNFTByTokenIdTestCase.text = "Query Failed";
        });
    }
}
