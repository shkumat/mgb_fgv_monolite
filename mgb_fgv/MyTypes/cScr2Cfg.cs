// ������ 1.03 �� 17 ������� 2017 �. - ������� ������������ �������2 �� Global.Cfg
namespace MyTypes
{
	public class	CScrooge2Config
	{
		bool		Is_Valid	;
		string		Err_Info	;
		string		Scrooge2Root;
		CCfgFile	CfgFile		;

		public	bool	IsValid
		{
			get
			{	return Is_Valid;
			}
		}

		public string ErrInfo
		{	
			get
			{	return Err_Info;
			}
		}

		public	string	this[string Key] {
			get {	
				// 2017.08.17 ��������� ��������� ������������, ������� ���������� � ������� ;
				string	Result	=	(string) CfgFile[Key.Trim().ToUpper()];
				if	( Result == null )
					return	CAbc.EMPTY;
				if	( Result.IndexOf(';') > 0 )
					Result	=	Result.Substring( 0 , Result.IndexOf(';')-1 );
				return	Result.Trim();
			}
		}

		public	CScrooge2Config()
		{
			Is_Valid	=	false;
			Scrooge2Root	=	CCommon.GetEnvStr("Scrooge2Root") ;
			if	( 	(Scrooge2Root == null) || (Scrooge2Root == "")	)
				{	Err_Info = "������ : �� ���������� ���������� ��������� Scrooge2Root !";
					return;
				}
			CfgFile		= new	CCfgFile( Scrooge2Root + "\\EXE\\GLOBAL.CFG" );
			if	(	( CfgFile["SERVER"] == null) || ( CfgFile["SERVER"] == "")	)
				{	Err_Info = "������ ������ ���������� �� ����� "+Scrooge2Root + "\\EXE\\GLOBAL.CFG";
					return;
				}
			Err_Info	=	"";
			Is_Valid	=	true;
		}	

	}
	/*
	public	class	MyDemo
	{
		public static void Main()
		{
			System.Diagnostics.Debugger.Launch();

			�Scrooge2Config Scrooge2Config = new �Scrooge2Config();

			if ( Scrooge2Config.IsValid )
			{
				System.Console.WriteLine( Scrooge2Config["ROOT"] );
			}
			else
			{
				System.Console.WriteLine( Scrooge2Config.ErrInfo );
			}
		}
	}
	*/
}