// ������ 2.02  �� 12 �������� 2014 �.  - ��� ������� ������������ �� Global.Erc

using MyTypes;

namespace MyTypes
{

	//-------------------------------------------------------------
	//              C�������� ����� � ���.����������� �� ���
	//
	// 1.�������� ����� �� ��� � !A-����� ,
	//   ������ ���������� ���������� � ���. ������� E
	//
	// 2.���� ��������� � ������ ������ �� ����� 255 ����.
	//
	// 3.��������� ������ ( ������� ���������� � 1 )
	//
	//      1..5            -       ����� ������ ��������� �-����� ,
	//                              � �������� ��������� ������ ������
	//
	//      6..15           -       ������� ����������� ���.��������� ,
	//                              DOCINFO ��� ���������� ������
	//
	//      16..255         -       �������� ���.���������
	//
	// 4. C�������� ��������� DocInfo :
	//
	// ��� ; �������� ��������� ; ����� ��������� ; ����� ��������� ;
	// ��� ����� �������� ; ����� ����� �������� ; ����� ;
	//
	// ������
	//
	// ������� ����� �����i���;�������;��;457038;���i����i������ ���������� � ����i����i� ������i 25/03/1997;28.01.1959;����i�, ���. �����i��i� 64 �� 10;;
	//
	public sealed class CErcEFileInfo
	{
		public static readonly int HEADER_SIZE = 196;
		public static readonly int RECORD_SIZE = 257;
		public static readonly int[] Header_Field_Size = { 12,6,176,2 };
		public static readonly int[] Record_Field_Size = { 5,10,240,2 };
		//------  ���� ���������			
		public const int H_FILENAME	= 0;	// char[ 12]; // �������� �����
		public const int H_DATE		= 1;	// char[ 6];  // ����
		public const int H_UNKNOWN	= 2;	// char[174]; // ���������� ����������
		public const int H_CRLF		= 3;	// char[  2]; // ������ `����� ������`
		//------  ���� �������������� ������			
		public const int L_LINENUM	= 0;	// char[  5]; // ����� ������ � �������� �����
		public const int L_PARAMCODE	= 1;	// char[ 10]; // ������� �������� ���������
		public const int L_INFO		= 2;	// char[240]; // �������� ���������
		public const int L_CRLF		= 3; 	// char[  2]; // ������ `����� ������`
	}

	public class CErcEReader : CDatReader
	{
		public override bool Open(string FileName, int CharSet)
		{
			HeaderFieldSize = CErcEFileInfo.Header_Field_Size;
			RecordFieldSize = CErcEFileInfo.Record_Field_Size;
			if ((base.Open(FileName, CharSet) == true)) {
				if ((RecordSize == CErcEFileInfo.RECORD_SIZE) & (HeaderSize == CErcEFileInfo.HEADER_SIZE)) {
					return true;
				}
			}
			return false;
		}
		/*
		public static void Main()
		{
			CErcEReader	ErcEReader;
        	        ErcEReader	= new	CErcEReader() ;

			if( ErcEReader.Open("D:\\!e4T_24J.002",CAbc.CHARSET_DOS) )
			{	while (ErcEReader.Read())
				{	CCommon.Print(	ErcEReader[	CErcEFileInfo.L_PARAMCODE	]
						+      " ( "  + ErcEReader[	CErcEFileInfo.L_LINENUM		]
						+      " ) = `"  + ErcEReader[	CErcEFileInfo.L_INFO		] +"`"
						);
					CCommon.Print("");
				}
			}
			ErcEReader.Close();
		}
		*/
	}

	//-------------------------------------------------------------
	public class CErcConfig
	{
		protected string	DirName					;
		protected string	FileName				;
		protected bool		Is_Valid				;
		protected string	Err_Info				;
		protected int		Erc_Date				;
		protected CCfgFile	CfgFile					;
		private	CTextReader	TxtReader				;
		private	CScrooge2Config Scrooge2Config				;

		public bool IsValid()
		{
			return Is_Valid;
		}

		public string ErrInfo()
		{
			return Err_Info;
		}

		public string LogDir()
		{
			return TodayDir() + "\\LOG\\";
		}

		public string TmpDir()
		{
			return TodayDir() + "\\TMP\\";
		}

		public virtual	string StatDir()
		{
			return TodayDir() + "\\SOS\\";
		}

		public	virtual	string	Config_FileName()
		{	return	"\\EXE\\GLOBAL.ERC";
		}

		public	virtual	string TodayDir()
		{	string	TmpS	=	CCommon.DtoC( Erc_Date );
			TmpS	=	TmpS.Substring(6, 2) + TmpS.Substring(4, 2) + TmpS.Substring(2, 2);
			return CfgFile["DaysDir"] + "\\" + TmpS + "\\";
		}

		public string this[string Key]
		{	get
			{	return	(string) CfgFile[ Key.Trim().ToUpper() ];
			}
		}

		public	int	ErcDate()
		{	return Erc_Date;
		}

		public	bool Open( int ErcDate )
		{	Erc_Date	=	ErcDate	;
			TxtReader	= new	CTextReader();
			Scrooge2Config	= new	CScrooge2Config();
			Is_Valid	=	Scrooge2Config.IsValid;
			Err_Info	=	Scrooge2Config.ErrInfo;
			if ( !IsValid() )
				return	false;
			CfgFile		= new	CCfgFile( Scrooge2Config["Root"] + "\\" + Config_FileName() );
			if	(	( CfgFile[ "ConfigDir" ] == null ) 
				||	( CfgFile[ "ConfigDir" ] == "" )
				)
			{	Err_Info = "������ ������ ���������� �� ����� " + Scrooge2Config["Root"] + Config_FileName() ;
				Is_Valid = false;
				return Is_Valid;
			}
			Is_Valid = Validate();
			return Is_Valid; 	
		}

		public bool Validate()
		{
			DirName = (string) CfgFile["InputDir"];
			Err_Info = "�� ������ ������� " + DirName;
			if (!CCommon.DirExists(DirName)) {
				if (!CCommon.MkDir(DirName))
					return false;
				if (!CCommon.DirExists(DirName))
					return false;
			}
			// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
			DirName = (string) CfgFile["OutputDir"];
			Err_Info = "�� ������ ������� " + DirName;
			if (!CCommon.DirExists(DirName)) {
				if (!CCommon.MkDir(DirName))
					return false;
				if (!CCommon.DirExists(DirName))
					return false;
			}
			// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
			DirName = (string) CfgFile["DaysDir"];
			Err_Info = "�� ������ ������� " + DirName;
			if (!CCommon.DirExists(DirName)) {
				if (!CCommon.MkDir(DirName))
					return false;
				if (!CCommon.DirExists(DirName))
					return false;
			}
			// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
			DirName = (string) CfgFile["FlagsDir"];
			Err_Info = "�� ������ ������� " + DirName;
			if (!CCommon.DirExists(DirName)) {
				if (!CCommon.MkDir(DirName))
					return false;
				if (!CCommon.DirExists(DirName))
					return false;
			}
			// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
			DirName = (string) CfgFile["ConfigDir"];
			Err_Info = "�� ������ ������� " + DirName;
			if (!CCommon.DirExists(DirName)) {
				if (!CCommon.MkDir(DirName))
					return false;
				if (!CCommon.DirExists(DirName))
					return false;
			}
			// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
			DirName = TodayDir();
			Err_Info = "�� ������ ������� " + DirName;
			if (!CCommon.DirExists(DirName)) {
				if (!CCommon.MkDir(DirName))
					return false;
				if (!CCommon.DirExists(DirName))
					return false;
			}
			// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
			DirName = StatDir();
			Err_Info = "�� ������ ������� " + DirName;
			if (!CCommon.DirExists(DirName)) {
				if (!CCommon.MkDir(DirName))
					return false;
				if (!CCommon.DirExists(DirName))
					return false;
			}
			// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
			DirName = LogDir();
			Err_Info = "�� ������ ������� " + DirName;
			if (!CCommon.DirExists(DirName)) {
				if (!CCommon.MkDir(DirName))
					return false;
				if (!CCommon.DirExists(DirName))
					return false;
			}
			// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
			DirName = TmpDir();
			Err_Info = "�� ������ ������� " + DirName;
			if (!CCommon.DirExists(DirName)) {
				if (!CCommon.MkDir(DirName))
					return false;
				if (!CCommon.DirExists(DirName))
					return false;
			}
			// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -			
			Err_Info = "";
			return true;

		}

	}

}
/*
public class App
{
	public static void Main()
	{
		Err.Debug();

		CErcConfig ErcConfig = new CErcConfig();

		if (ErcConfig.IsValid()) {
			System.Console.WriteLine(ErcConfig["InputDir"]);
		} else {
			System.Console.WriteLine(ErcConfig.ErrInfo());
		}
	}
}
*/