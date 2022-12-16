using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class PRead
{
	public PRead(string fn)
	{
		this.fs = new FileStream(fn, FileMode.Open, FileAccess.Read);
		this.Init();
		if (fn.ToLower().EndsWith("adult.dat"))
		{
			this.ti.Remove("def/version.txt");
		}
	}

	public void Release()
	{
		if (this.fs != null)
		{
			this.fs.Close();
			this.fs = null;
		}
	}

	~PRead()
	{
		this.Release();
	}

	private void Init()
	{
		this.ti = new Dictionary<string, PRead.fe>();
		this.fs.Position = 0L;
		byte[] array = new byte[1024];
		this.fs.Read(array, 0, 1024);
		int num = 0;
		for (int i = 3; i < 255; i++)
		{
			num += BitConverter.ToInt32(array, i * 4);
		}
		byte[] array2 = new byte[16 * num];
		this.fs.Read(array2, 0, array2.Length);
		this.dd(array2, 16 * num, BitConverter.ToUInt32(array, 212));
		int num2 = BitConverter.ToInt32(array2, 12) - (1024 + 16 * num);
		byte[] array3 = new byte[num2];
		this.fs.Read(array3, 0, array3.Length);
		this.dd(array3, num2, BitConverter.ToUInt32(array, 92));
		this.Init2(array2, array3, num);
	}
	protected void Init2(byte[] rtoc, byte[] rpaths, int numfiles)
	{
		int num = 0;
		for (int i = 0; i < numfiles; i++)
		{
			int num2 = 16 * i;
			uint l = BitConverter.ToUInt32(rtoc, num2);
			int num3 = BitConverter.ToInt32(rtoc, num2 + 4);
			uint k = BitConverter.ToUInt32(rtoc, num2 + 8);
			uint p = BitConverter.ToUInt32(rtoc, num2 + 12);
			int num4 = num3;
			while (num4 < rpaths.Length && rpaths[num4] != 0)
			{
				num4++;
			}
			string key = Encoding.ASCII.GetString(rpaths, num, num4 - num).ToLower();
			PRead.fe value = default(PRead.fe);
			value.p = p;
			value.L = l;
			value.k = k;
			this.ti.Add(key, value);
			num = num4 + 1;
		}
	}
	private void gk(byte[] b, uint k0)
	{
		uint num = k0 * 4892U + 42816U;
		uint num2 = num << 7 ^ num;
		for (int i = 0; i < 256; i++)
		{
			num -= k0;
			num += num2;
			num2 = num + 156U;
			num *= (num2 & 206U);
			b[i] = (byte)num;
			num >>= 3;
		}
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003AA8 File Offset: 0x00001CA8
	protected void dd(byte[] b, int L, uint k)
	{
		byte[] array = new byte[256];
		this.gk(array, k);
		for (int i = 0; i < L; i++)
		{
			byte b2 = b[i];
			b2 ^= array[i % 179];
			b2 += 3;
			b2 += array[i % 89];
			b2 ^= 119;
			b[i] = b2;
		}
	}
	public virtual byte[] Data(string fn)
	{
		PRead.fe fe;
		if (!this.ti.TryGetValue(fn, out fe))
		{
			return null;
		}
		this.fs.Position = (long)((ulong)fe.p);
		byte[] array = new byte[fe.L];
		this.fs.Read(array, 0, array.Length);
		this.dd(array, array.Length, fe.k);
		return array;
	}

	private FileStream fs;

	public Dictionary<string, PRead.fe> ti;

	public struct fe
	{
		public uint p;
		public uint L;
		public uint k;
	}
}
