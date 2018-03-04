using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorityDisplay : ResourceTextDisplay
{

    void LateUpdate()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("Authority : ").Append(village.Authority.ToString());
        text.text = sb.ToString();
    }
}
