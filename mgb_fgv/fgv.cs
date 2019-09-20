// ������ 1.03 �� 12.03.2018�. ����������� ��������� ��� ����� ��������������
// encoding=cp-1251
/*
		��������� ��������� :

	-date		�������� ���� - �������� 2016.09.20 ,
			���� �� �������, �� �������� ���� = ������� ;
	-sep		����������� ����� ��� csv ����� ( C / s / t )
	-cor		��������� �� �������������� �������� ( y / N )
	-mode		����� ����� �������
		n63	�� ��. 2903
		n64	�� ��. 2620,2622,2625,2628,2630....
		n65	�� ��. 2600,2602,2603,2604,2605,2608,2610,2615,2618

	������ ������ ���������� :
	-date 2016.09.20  -mode n64
*/
using	__	=	MyTypes.CCommon ;
using	MyTypes;

public	class	Fgv {
	static	bool	DEBUG		=	false	;
	static	CConnection Connection			;
	static	bool	NeedColumnNames	=	true	;
	static	int	CharSet		=	CAbc.CHARSET_WINDOWS	;
	static	int[]	MetaData	=	{ 44 }		;

	public	static	void	PrintAboutMe() {
		__.Print(""," ����������� ��������� ��� ����� ��������������. ������ 1.03 �� 12.03.2018�.");
		__.Print("\t\t\t��������� ��������� :");
		__.Print("\t-date\t\t�������� ���� - �������� 2016.09.20 ");
		__.Print("\t\t\t���� �� �������, �� �������� ���� = ������� ;");
		__.Print("\t-sep\t\t����������� ����� ��� csv ����� ( C / s / t )");
		__.Print("\t-cor\t\t��������� �� �������������� �������� ( y / N )");
		__.Print("\t-mode\t\t����� ����� �������");
		__.Print("\t\tn63\t�� ��. 2903");
		__.Print("\t\tn64\t�� ��. 2620,2622,2625,2628,2630....");
		__.Print("\t\tn65\t�� ��. 2600,2602,2603,2604,2605,2608,2610,2615,2618");
		__.Print("");
		__.Print("\t������ ������ ���������� :");
		__.Print("\t-date 2016.09.20  -mode n64");
		__.Print("");
	}

	static	void WriteDataToCsv( string CommandText , string FileName ){
		bool	DecimalPoint	=	( MetaData[0] == 44 )	;
		if	( DEBUG )
			__.Print( CommandText );
		IFileOfColumnsWriter	FileOfColumnsWriter	= new	CCsvWriter();
		if	( ! FileOfColumnsWriter.Create( FileName , CharSet , MetaData ) ) {
			__.Print( "������ �������� ����� " + FileName  );
			return;
		}
		__.Print( "������ ����� � ���� " + FileName  );
		CRecordSet	RecordSet	= new	CRecordSet( Connection ) ;
		if	( RecordSet.Open( CommandText ) ) {
			if	( RecordSet.Read() ) {
				int	FieldCount	=	RecordSet.FieldCount();
				if	( NeedColumnNames ) {
					for	( int Index=0; Index<FieldCount; Index++ )
					 	FileOfColumnsWriter.Write( RecordSet.GetName( Index ) ) ;
					FileOfColumnsWriter.WriteLine();
				}
				do	{
					for	( int Index=0; Index<FieldCount; Index++ ) {
						string	CurValue	=	RecordSet[ Index ];
						if	( __.IsDigitEx( CurValue ) )
							if	( DecimalPoint )
								CurValue=CurValue.Replace(",",".");
							else
								CurValue=CurValue.Replace(".",",");
					 	FileOfColumnsWriter.Write( CurValue ) ;
					 }
					FileOfColumnsWriter.WriteLine();
				} while	( RecordSet.Read() );
			}
		}
		RecordSet.Close();
		FileOfColumnsWriter.Close();
	}

	static void Main()  {
		string	ScroogeDir	=	CAbc.EMPTY;
		string	ServerName	=	CAbc.EMPTY;
		string	DataBase	=	CAbc.EMPTY;
		string	ConnectionString=	CAbc.EMPTY;
		if	( ! DEBUG )
			if	( __.ParamCount() < 2 ) {
				PrintAboutMe();
				return;
			}
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		CCommon.Print( ""," ����������� ��������� ��� ����� ��������������. ������ 1.03 �� 12.03.2018�." ,"") ;
		CScrooge2Config	Scrooge2Config	= new	CScrooge2Config();
		if (!Scrooge2Config.IsValid) {
			CCommon.Print( Scrooge2Config.ErrInfo ) ;
			return;
		}
		ScroogeDir	=	(string)Scrooge2Config["Root"];
		ServerName	=	(string)Scrooge2Config["Server"];
		DataBase	=	(string)Scrooge2Config["DataBase"];
		if( ScroogeDir == null ) {
			CCommon.Print("  �� ������� ���������� `Root` � ���������� `������-2` ");
			return;
		}
		if( ServerName == null ) {
			CCommon.Print("  �� ������� ���������� `Server` � ���������� `������-2` ");
			return;
		}
		if( DataBase == null ) {
			CCommon.Print("  �� ������� ���������� `Database` � ���������� `������-2` ");
			return;
		}
		CCommon.Print("  ���� ��������� `������-2` ����� :  " + ScroogeDir );
		__.Print("  ������        :  " + ServerName  );
		__.Print("  ���� ������   :  " + DataBase + CAbc.CRLF );
		ConnectionString	=	"Server="	+	ServerName
					+	";Database="	+	DataBase
					+	";Integrated Security=TRUE;"
					;
		Connection		= new CConnection( ConnectionString ) ;
		if      ( ! Connection.IsOpen() ) {
			CCommon.Print("  ������ ����������� � ��������� ������ !");
			return;
		}
		// - - - - - - - - - - - - - - - - - - - - - - - - - - - -
		string		TODAY_STR	=	CCommon.StrD( CCommon.Today() , 10,10).Substring(6)
						+	CCommon.StrD( CCommon.Today() , 10,10).Substring(2,4)
						+	CCommon.StrD( CCommon.Today() , 10,10).Substring(0,2);
		CParam		Param		= new	CParam();
		if	( ! __.IsEmpty( Param["DEBUG"] ) )
			DEBUG	=	true;
		bool	NeedCorrection	=	( ( Param["COR"] ).ToUpper() == "Y" );
		string	Date;
		if	( __.IsEmpty( Param["DATE"] ) )
			Date		=	TODAY_STR;
		else
			Date		=	( Param["DATE"] );
		string	DateStr		=	Date.Trim().Replace(",","").Replace(".","").Replace("/","");
		if	( ! __.IsEmpty( Param["SEP"] ) )
			switch	( Param["SEP"].ToUpper()[0] ) {
				case	'C': {
					MetaData[0]	=	44;
					break;
				}
				case	'S': {
					MetaData[0]	=	59;
					break;
				}
				case	'T': {
					MetaData[0]	=	9;
					break;
				}
				default	: {
					__.Print("������ ����������� ����������� �����.");
					break;
				}
			}
		if	( __.IsEmpty( Param["MODE"] ) )
				__.Print("�� ������� `mode` - ����� ����� ������� .");
		else
			switch	( Param["MODE"].ToUpper().Trim() ) {
				case	"N63": {
					WriteDataToCsv(
							"exec dbo.Mega_Report_SaldoFGV;2 '"+Date
							+"','2903'"
							+ ( NeedCorrection ? ",1" : "" )
						,	DateStr + 	"-63.csv"
					);
					break;
				}
				case	"N64": {
					WriteDataToCsv(
							"exec dbo.Mega_Report_SaldoFGV;2 '"+Date
							+"','2620,2622,2625,2628,2630,2635,2638,2903,3320,3328,3330,3338,3340,3348'"
							+ ( NeedCorrection ? ",1" : "" )
						,	DateStr + ".csv"
					);
					break;
				}
				case	"N65": {
					WriteDataToCsv(
							"exec dbo.Mega_Report_SaldoFGV;2 '"+Date
							+"','2600,2602,2603,2604,2605,2608,2610,2615,2618'"
							+ ( NeedCorrection ? ",1" : "" )
						,	DateStr + "-65.csv"
					);
					break;
				}
				default	: {
					__.Print("������ ������������ ��� ������.");
					break;
				}
			}
		if	( DEBUG )
			WriteDataToCsv(
					"exec dbo.Mega_Report_SaldoFGV;2 '2015.12.01','2903',1"
				,	"20160801-63.csv"
			);
		Connection.Close();
		__.Print("������.");
		return;
	}
}