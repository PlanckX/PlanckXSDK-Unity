using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownController : MonoBehaviour {
    public GameObject bindTestCase;
    public GameObject getAllGameNFTsTestCase;
    public GameObject getAllPlayerNFTsTestCase;
    public GameObject getNFTByTokenIdTest;
    public void OnDropDownCbSelectTestCase(int caseNum) {
        bindTestCase.SetActive(false);
        getAllGameNFTsTestCase.SetActive(false);
        getAllPlayerNFTsTestCase.SetActive(false);
        getNFTByTokenIdTest.SetActive(false);
        switch (caseNum) {
            case 0:
                bindTestCase.SetActive(true);
                break;
            case 1:
                getAllGameNFTsTestCase.SetActive(true);
                break;
            case 2:
                getAllPlayerNFTsTestCase.SetActive(true);
                break;
            case 3:
                getNFTByTokenIdTest.SetActive(true);
                break;
        }
    }
}
