Ñ
UC:\Repositorios\Cifrado.ini\sa-mti-crypto\sa-mti-crypto.Domain\Dto\OperationResult.cs
	namespace 	
sa_mti_crypto
 
. 
Domain 
. 
Dto "
{ 
public 

sealed 
class 
OperationResult '
{ 
public 
bool 
Success 
{ 
get !
;! "
init# '
;' (
}) *
public 
string 
? 
Message 
{  
get! $
;$ %
init& *
;* +
}, -
public 
string 
? 
OutputFilePath %
{& '
get( +
;+ ,
init- 1
;1 2
}3 4
} 
}		 À
WC:\Repositorios\Cifrado.ini\sa-mti-crypto\sa-mti-crypto.Domain\Contracts\IKeyManager.cs
	namespace 	
sa_mti_crypto
 
. 
Domain 
. 
	Contracts (
{ 
public 

	interface 
IKeyManager  
{ 
byte 
[ 
] !
DeriveKeyFromPassword $
($ %
string% +
password, 4
,4 5
byte6 :
[: ;
]; <
salt= A
)A B
;B C
byte 
[ 
] 
GenerateSalt 
( 
) 
; 
} 
} ‚
^C:\Repositorios\Cifrado.ini\sa-mti-crypto\sa-mti-crypto.Domain\Contracts\IEncryptRepository.cs
	namespace 	
sa_mti_crypto
 
. 
Domain 
. 
	Contracts (
{ 
public 

	interface 
IEncryptRepository '
{ 
byte 
[ 
] 
EncryptData 
( 
FileInfo #
fileInfo$ ,
,, -
string. 4
password5 =
)= >
;> ?
byte 
[ 
] 
EncryptData 
( 
string !
	plainText" +
,+ ,
string- 3
password4 <
)< =
;= >
byte 
[ 
] 
EncryptData 
( 
byte 
[  
]  !
data" &
,& '
string( .
password/ 7
)7 8
;8 9
} 
}		 ±
^C:\Repositorios\Cifrado.ini\sa-mti-crypto\sa-mti-crypto.Domain\Contracts\IDecryptRepository.cs
	namespace 	
sa_mti_crypto
 
. 
Domain 
. 
	Contracts (
{ 
public 

	interface 
IDecryptRepository '
{ 
byte 
[ 
] 
DecryptData 
( 
byte 
[  
]  !
encryptedData" /
,/ 0
string1 7
password8 @
)@ A
;A B
byte 
[ 
] 
DecryptData 
( 
string !

base64data" ,
,, -
string. 4
password5 =
)= >
;> ?
} 
} 