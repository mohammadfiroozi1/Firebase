using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum ContentType
{
    image, video, audio, model, pdf
}
public class Step : MonoBehaviour
{
    [Header("Content Options")]
    public GameObject imagePanel, videoPanel, audioPanel, modelPanel, pdfPanel;
    public RawImage rawImage;
    public AudioClip audioClip;
    public GameObject pdf;
    public GameObject model;

    [Space]
    public string narriation;
    public Step nextStep;
    public Step previousStep;

}
