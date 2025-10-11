using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for E_QrCodeResponse1
/// </summary>
public class E_QrCodeResponse1
{
    private string _status;
    public string Status
    {
        get { return _status; }
        set { _status = value; }
    }

    private string _qr_code;
    public string QrCode
    {
        get { return _qr_code; }
        set { _qr_code = value; }
    }

    private string _payment_uri;
    public string PaymentUri
    {
        get { return _payment_uri; }
        set { _payment_uri = value; }
    }

    private string _imgsrc;
    public string Imgsrc
    {
        get { return _imgsrc; }
        set { _imgsrc = value; }
    }

    private string _QrCodeAddress;
    public string QrCodeAddress
    {
        get { return _QrCodeAddress; }
        set { _QrCodeAddress = value; }
    }
}
public class Callbacks1
{
    private string _result;
    public string Result
    {
        get { return _result; }
        set { _result = value; }
    }

    private string _txid_in;
    public string TxidIn
    {
        get { return _txid_in; }
        set { _txid_in = value; }
    }

    private string _txid_out;
    public string TxidOut
    {
        get { return _txid_out; }
        set { _txid_out = value; }
    }

    private string _value_coin;
    public string ValueCoin
    {
        get { return _value_coin; }
        set { _value_coin = value; }
    }

    private string _message;
    public string Message
    {
        get { return _message; }
        set { _message = value; }
    }
}

public class M_QRCode
{
    private E_QrCodeResponse1 _QrCode;
    public E_QrCodeResponse1 QrCode
    {
        get { return _QrCode; }
        set { _QrCode = value; }
    }

    private List<Callbacks1> _call;
    public List<Callbacks1> Call
    {
        get { return _call; }
        set { _call = value; }
    }
}

public class CallbacksRes
{
    private string _result;
    public string Result
    {
        get { return _result; }
        set { _result = value; }
    }

    private string _txid_in;
    public string TxidIn
    {
        get { return _txid_in; }
        set { _txid_in = value; }
    }

    private string _txid_out;
    public string TxidOut
    {
        get { return _txid_out; }
        set { _txid_out = value; }
    }

    private string _value_coin;
    public string ValueCoin
    {
        get { return _value_coin; }
        set { _value_coin = value; }
    }

    private string _message;
    public string Message
    {
        get { return _message; }
        set { _message = value; }
    }

    private string _status;
    public string Status
    {
        get { return _status; }
        set { _status = value; }
    }

    private string _response;
    public string Response
    {
        get { return _response; }
        set { _response = value; }
    }
    private string _balance;
    public string balance
    {
        get { return _balance; }
        set { _balance = value; }
    }
    private string _Rembalance;
    public string Rembalance
    {
        get { return _Rembalance; }
        set { _Rembalance = value; }
    }
    private string _paymentid;
    public string PaymentID
    {
        get { return _paymentid; }
        set { _paymentid = value; }
    }

    private string _callbackurl;
    public string CallbackUrl
    {
        get { return _callbackurl; }
        set { _callbackurl = value; }
    }
}

public class TransactionRecord
{
    public string UserId { get; set; }
    public string RecordId { get; set; }
    public int CoinId { get; set; }
    public string Chain { get; set; }
    public string Contract { get; set; }
    public string CoinSymbol { get; set; }
    public string TxId { get; set; }
    public string CoinUSDPrice { get; set; }
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public string ToMemo { get; set; }
    public decimal Amount { get; set; }
    public decimal ServiceFee { get; set; }
    public string Status { get; set; }
    public long ArrivedAt { get; set; }
    public bool IsFlaggedAsRisky { get; set; }
}