using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class SecurityCertificatePolicy
{
	public static bool Validator(
		object sender,
		X509Certificate certificate,
		X509Chain chain,
		SslPolicyErrors policyErrors) 
	{
		// Just accept and move on...
		return true;	
	}
	
	public static void Instate() 
	{
		ServicePointManager.ServerCertificateValidationCallback = Validator;
	}
}
