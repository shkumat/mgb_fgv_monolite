// ������ 2.01 �� 8 ������ 2019�. - ������ ��� ������ � ���������� �������
using MyTypes;

namespace MyTypes {

	public sealed class CSepAFileInfo {
	
		public static int HEADER_SIZE = 298;
		public static int RECORD_SIZE = 594;
		public static readonly int[] Header_Field_Size = { 100,2,12,6,4,6,16,16,64,6,64,2};
		public static readonly int[] Record_Field_Size = { 9,14,9,14,1,16,2,10,3,6,6,38,38,160,20,20,20,3,2,14,14,9,16,64,34,34,16,2 };
		//------  ���� ���������			
		public const int H_EMPTYSTR	= 0;	// char[100]; // ��p��� 100 - �p����� 			
		public const int H_CRLF1	= 1;	// char[  2]; // ������ ������ ������                       			
		public const int H_FILENAME	= 2;	// char[ 12]; // ������������  �����			
		public const int H_DATE 	= 3; 	// char[  6]; // ���� �������� �����    			
		public const int H_TIME		= 4; 	// char[  4]; // ���� �������� �����    			
		public const int H_STRCOUNT	= 5;	// char[  6]; // ���������� �� � �����  			
		public const int H_TOTALDEBET	= 6;	// char[ 16]; // ����� ������ �� �����  			
		public const int H_TOTALCREDIT	= 7;	// char[ 16]; // ����� �p����� �� ����� 			
		public const int H_DES		= 8;	// char[ 64]; // ���                    			
		public const int H_DES_ID	= 9;	// char[  6]; // ID ����� ���           			
		public const int H_DES_OF_HEADER=10;	// char[ 64]; // ��� ���������          			
		public const int H_CRLF2	=11;	// char[  2]; // ������ `����� ������`
		//------  ���� �������������� ������			
		public const int L_DEBITMFO	= 0;	// char[  9]; // �����-���			
		public const int L_DEBITACC	= 1;	// char[ 14]; // �����-����			
		public const int L_CREDITMFO	= 2;	// char[  9]; // ������-���			
		public const int L_CREDITACC	= 3;	// char[ 14]; // ������ ����			
		public const int L_FLAG		= 4;	// char[  1]; // ���� `�����/������`			
		public const int L_SUMA		= 5;	// char[ 16]; // ����� � ��������			
		public const int L_DTYPE	= 6;	// char[  2]; // ��� ���������			
		public const int L_NDOC		= 7;	// char[ 10]; // ����� ���������			
		public const int L_CURRENCY	= 8;	// char[  3]; // ������			
		public const int L_DATE1	= 9;	// char[  6]; // ���� �������			
		public const int L_DATE2	=10;	// char[  6]; // ���� ����������� ���������			
		public const int L_DEBITNAME	=11;	// char[ 38]; // ������������ �����-�����			
		public const int L_CREDITNAME	=12;	// char[ 38]; // ������������ ������-�����			
		public const int L_PURPOSE	=13;	// char[160]; // ���������� �������			
		public const int L_RESERVED1	=14;	// char[ 20]; // (������)  			
		public const int L_DEBITACC_EXT	=15;	// char[ 20]; // ����������� ������� ���� ������� � 			
		public const int L_CREDITACC_EXT=16;	// char[ 20]; // ����������� ������� ���� ������� � 			
		public const int L_SYMBOL	=17;	// char[  3]; // �������� ������			
		public const int L_RESERVED2	=18;	// char[  2]; // (������)			
		public const int L_OKPO1	=19;	// char[ 14]; // �����.��� ������� �			
		public const int L_OKPO2	=20;	// char[ 14]; // �����.��� ������� �			
		public const int L_ID		=21;	// char[  9]; // ������������� ���������			
		public const int L_RESERVED3	=22; 	// char[ 16]; // (������)			
		public const int L_DES		=23; 	// char[ 64]; // ���			
		public const int L_DEBITIBAN	=24;	// char[ 34]; // �����-IBAN		
		public const int L_CREDITIBAN	=25;	// char[ 34]; // ������-IBAN		
		public const int L_RESERVED4	=26;	// char[ 16]; // (������)		
		public const int L_CRLF		=27; 	// char[  2]; // ������ `����� ������`
	}

	public class CSepAReader : CDatReader	, IFileOfColumnsReader {
		bool	IFileOfColumnsReader.Read() {
			return	base.Read();
                }

		string	IFileOfColumnsReader.this[ int Index ] {
	        get	{	
                        	return	base[ Index ];
                	}
                }

		int	 IFileOfColumnsReader.FieldCount {
        	get	{	
				return	CSepAFileInfo.Record_Field_Size.Length;
			}
	        }

		int	 IFileOfColumnsReader.Count {
        	get	{	
				return	CCommon.CInt32( Head( CSepAFileInfo.H_STRCOUNT ) );
			}
	        }

		bool IFileOfColumnsReader.Open( string FileName , int CharSet , params int[] MetaData ) {
			return	Open( FileName , CharSet );
		}

		public override bool Open(string FileName, int CharSet) {
			HeaderFieldSize = CSepAFileInfo.Header_Field_Size;
			RecordFieldSize = CSepAFileInfo.Record_Field_Size;
			if ((base.Open(FileName, CharSet) == true)) {
				if ((RecordSize == CSepAFileInfo.RECORD_SIZE) & (HeaderSize == CSepAFileInfo.HEADER_SIZE)) {
					return true;
				}
			}
			return false;
		}

	}

	public class CSepAWriter : CDatWriter {
		public override bool Create(string FileName, int CharSet) {
			HeaderFieldSize = CSepAFileInfo.Header_Field_Size;
			RecordFieldSize = CSepAFileInfo.Record_Field_Size;
			if ((base.Create(FileName, CharSet) == true)) {
				if ((RecordSize == CSepAFileInfo.RECORD_SIZE) & (HeaderSize == CSepAFileInfo.HEADER_SIZE)) {
					return true;
				}
			}
			return false;
		}

		public override bool OpenForAppend(string FileName, int CharSet) {
			HeaderFieldSize = CSepAFileInfo.Header_Field_Size;
			RecordFieldSize = CSepAFileInfo.Record_Field_Size;
			if ((base.OpenForAppend(FileName, CharSet) == true)) {
				if ((RecordSize == CSepAFileInfo.RECORD_SIZE) & (HeaderSize == CSepAFileInfo.HEADER_SIZE)) {
					return true;
				}
			}
			return false;
		}
	}

	// ----------------------------------------------------------------------
	public sealed class CSepVFileInfo {
		public static int HEADER_SIZE = 529;
		public static int RECORD_SIZE = 138;
		public static readonly int[] Header_Field_Size = { 100,2,12,6,4,6,16,16,365,2 }	;
		public static readonly int[] Record_Field_Size = { 9,14,9,14,1,16,2,10,3,6,9,12,6,12,6,1,4,1,1,2 };
		//------  ���� ���������
		public const int H_EMPTYSTR	= 0;	// char[100]; // ��p��� 100 - �p�����
		public const int H_CRLF1	= 1;	// char[  2]; // ������� ����� ������
		public const int H_FILENAME	= 2;	// char[ 12]; // ��� �����
		public const int H_DATE 	= 3; 	// char[  6]; // ���� �������� �����
		public const int H_TIME		= 4; 	// char[  4]; // ����� �������� �����
		public const int H_STRCOUNT	= 5;	// char[  6]; // ���������� �� � �����
		public const int H_TOTALDEBET	= 6;	// char[ 16]; // ����� ������ �� �����
		public const int H_TOTALCREDIT	= 7;	// char[ 16]; // ����� �p����� �� �����
		public const int H_EMPTYSTR2	= 8;    // char[365]; // �������������� ������������
		public const int H_CRLF2	= 9;	// char[  2]; // ������� ����� ������
		//------  ���� �������������� ������
		public const int L_DEBITMFO	= 0;	// char[  9]; // �����-���
		public const int L_DEBITACC	= 1;	// char[ 14]; // �����-����
		public const int L_CREDITMFO	= 2;	// char[  9]; // ������-���
		public const int L_CREDITACC	= 3;	// char[ 14]; // ������ ����
		public const int L_FLAG		= 4;	// char[  1]; // ���� `�����/������`
		public const int L_SUMA		= 5;	// char[ 16]; // ����� � ��������
		public const int L_DTYPE	= 6;	// char[  2]; // ��� ���������
		public const int L_NDOC		= 7;	// char[ 10]; // ����� ���������
		public const int L_CURRENCY	= 8;	// char[  3]; // ������
		public const int L_DATE1	= 9;	// char[  6]; // ���� �������
		public const int L_ID		=10;	// char[  9]; // ������������� ���������
		public const int L_FILENAME1	=11;	// char[ 12]; // ��� ����� N 1
		public const int L_LINENUM1	=12;	// char[  6]; // ����� ������ � ����� N 1
		public const int L_FILENAME2	=13;	// char[ 12]; // ��� ����� N 2
		public const int L_LINENUM2	=14;	// char[  6]; // ����� ������ � ����� N 2
		public const int L_STATUS	=15;	// char[  1]; // ���� �������� �������
		public const int L_TIME		=16;	// char[  4]; // �����
		public const int L_NOL1		=17;	// char[  1]; //
		public const int L_NOL2		=18;	// char[  1]; //
		public const int L_CRLF		=19;	// char[  2]; // ������� ����� ������
	}

	public class CSepVWriter : CDatWriter {

		public override bool Create(string FileName, int CharSet) {
			HeaderFieldSize = CSepVFileInfo.Header_Field_Size;
			RecordFieldSize = CSepVFileInfo.Record_Field_Size;
			if ( base.Create(FileName, CharSet) )
				if ( ( RecordSize == CSepVFileInfo.RECORD_SIZE ) && (HeaderSize == CSepVFileInfo.HEADER_SIZE) )
					return true;
			return false;
		}

		public override bool OpenForAppend(string FileName, int CharSet) {
			HeaderFieldSize = CSepVFileInfo.Header_Field_Size;
			RecordFieldSize = CSepVFileInfo.Record_Field_Size;
			if ( base.OpenForAppend(FileName, CharSet) == true)
				if ((RecordSize == CSepVFileInfo.RECORD_SIZE) & (HeaderSize == CSepVFileInfo.HEADER_SIZE))
					return true;
			return false;
		}
	}

/*
	public	class	MyDemo {

		public static void Main() {
			//Err.Debug();

			string	FILE_NAME	=	"D:\\!BCGC310.Z20";
			
			CSepAReader	A	= new	CSepAReader();
			if ( A.Open( FILE_NAME , 866 ) )
				while( A.Read() )
					CCommon.Print( A[ CSepAFileInfo.L_PURPOSE ] );
			A.Close();			
		}
	}
*/
}