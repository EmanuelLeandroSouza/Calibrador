using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calibrador
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            rtbLog.AppendText("Iniciando conexão (simulação)...\n");
            btnConectar.Enabled = false;
            btnDesconectar.Enabled = true;
            btnCalibrarEscala.Enabled = true;
            rtbLog.AppendText("Conectado com sucesso! (simulação)\n");
        }
    }
}
