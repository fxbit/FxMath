using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

using FxMaths;
using FxMaths.Matrix;
using FxMaths.GUI;
using FxMaths.Vector;


using NAudio.Wave;
using NAudio.Wave.SampleProviders;

using FxMaths.DSP;
using System.IO;


namespace FXMaths_Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        #region FFT Demo

        FxMaths.Vector.FxVectorF signal;
        FxMaths.Vector.FxVectorF dftReal,dftImag;
        FxMaths.Vector.FxVectorF re_signal;

        private float SignalGenerator( float t )
        {
            return (float)( Math.Cos( t * Math.PI / 2 ) + Math.Cos( t * Math.PI / 5 ) ) * 20.0f;
        }

        private void button4_Click( object sender, EventArgs e )
        {
            // init the signal
            signal = new FxMaths.Vector.FxVectorF( 1024, new Fx1DGeneratorF( SignalGenerator ), 0.1f );

            // create a plot base on signal
            PloterElement plot = new PloterElement( signal );
            plot.Position.X = 0;
            plot.Position.Y = 50;
            plot.Origin.X = 10;
            plot.Origin.Y = 100;
            plot.FitPlots();

            // add the signal to canva
            Signal_Canva.AddElements( plot );

            // add text for the signal plot
            TextElement text= new TextElement( "Signal Plot:" );
            text.Position.X = 10;
            text.Position.Y = 10;

            // add the text to canva
            Signal_Canva.AddElements( text );

        }

        private void button3_Click( object sender, EventArgs e )
        {
            // calc the DFT of the signal
            //SignalTools.DFT_F( signal, new FxVectorF( signal.Size ), true, out dftReal, out dftImag );
            SignalTools.FFT_Safe_F( signal, new FxVectorF( signal.Size ), true, out dftReal, out dftImag );

            // create a plot base on real part of DFT
            PloterElement plotDFT_Real = new PloterElement( dftReal );
            plotDFT_Real.Position.X = 0;
            plotDFT_Real.Position.Y = 450;
            plotDFT_Real.Origin.X = 10;
            plotDFT_Real.Origin.Y = 100;
            plotDFT_Real.FitPlots();

            // add the signal to canva
            Signal_Canva.AddElements( plotDFT_Real );

            // add text for the signal plot
            TextElement text= new TextElement( "FFT Real:" );
            text.Position.X = 10;
            text.Position.Y = 400;

            // add the text to canva
            Signal_Canva.AddElements( text );

            // ----------------------------------------------------------------------------------------------- //

            // create a plot base on  imag part of DFT
            PloterElement plotDFT_Imag = new PloterElement( dftImag );
            plotDFT_Imag.Position.X = 600;
            plotDFT_Imag.Position.Y = 450;
            plotDFT_Imag.Origin.X = 10;
            plotDFT_Imag.Origin.Y = 100;
            plotDFT_Imag.FitPlots();

            // add the signal to canva
            Signal_Canva.AddElements( plotDFT_Imag );

            // add text for the signal plot
            text = new TextElement( "FFT Imag:" );
            text.Position.X = 610;
            text.Position.Y = 400;

            // add the text to canva
            Signal_Canva.AddElements( text );
        }

        private void button5_Click( object sender, EventArgs e )
        {
            // calc the DFT of the signal
            //SignalTools.DFT_F( dftReal, dftImag, true, out re_signal, out dftImag );
            SignalTools.FFT_Safe_F( dftReal, dftImag, true, out re_signal, out dftImag );

            // create a plot base on signal
            PloterElement plot = new PloterElement( re_signal );
            plot.Position.X = 600;
            plot.Position.Y = 50;
            plot.Origin.X = 10;
            plot.Origin.Y = 100;
            plot.FitPlots();

            // add the signal to canva
            Signal_Canva.AddElements( plot );

            // add text for the signal plot
            TextElement text = new TextElement( "Recostructed Signal:" );
            text.Position.X = 610;
            text.Position.Y = 10;

            // add the text to canva
            Signal_Canva.AddElements( text );
        }

        private void button6_Click( object sender, EventArgs e )
        {
            FxVectorF convResultFFT;
            FxVectorF convResultDFT;

            // create a filter for testing
            FxVectorF filter = new FxVectorF( 1024 );
            filter[0] = 0.1f;
            filter[1] = 0.5f;
            filter[2] = 1.5f;
            filter[3] = 0.5f;
            filter[4] = 0.1f;
            TimeStatistics.StartClock();

            // execute the conv
            SignalTools.Convolution_FFT_F( signal, filter, out convResultFFT );
            //SignalTools.Convolution_DFT_F( signal, filter, out convResultDFT );

            TimeStatistics.StopClock( 1 );

            // create a plot base on signal
            PloterElement plot = new PloterElement( signal );
            plot.Position.X = 600;
            plot.Position.Y = 50;
            plot.Origin.X = 10;
            plot.Origin.Y = 100;
            plot.FitPlots();

            // add the signal to the same plot
            plot.AddPlot( convResultFFT, PlotType.Lines, Color.RoyalBlue );
            //plot.AddPlot( convResultDFT, PlotType.Lines, Color.MistyRose );

            // add the signal to canva
            Signal_Canva.AddElements( plot );

            // add text for the signal plot
            TextElement text = new TextElement( "Conv Signal:" );
            text.Position.X = 610;
            text.Position.Y = 10;

            // add the text to canva
            Signal_Canva.AddElements( text );

        }

        #endregion


        #region Audio

        private IWavePlayer waveOut;
        private string audioFileName = null;
        private WaveStream fileWaveStream;

        private void button8_Click( object sender, EventArgs e )
        {
            openFileDialog1.Filter = "All Supported Files|*.mp3";
            openFileDialog1.FilterIndex = 1;

            if ( openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
                
                // set the file name
                audioFileName = openFileDialog1.FileName;
            }
        }

        float vol = 1.0f;
        FxMaths.Vector.FxVectorF signal_spectrum;
        PloterElement plot_signal_spectrum_original;
        PloterElement plot_signal_spectrum;
        PloterElement plot_filter_spectrum;

        Boolean firstTime= true;
        Boolean useFilter= true;
        Boolean ShowInputAudioSpectrum=false;
        Boolean ShowOutputAudioSpectrum=false;

        FxVectorF filterVec;
        FxVectorF tmpVecOrig;
        FxVectorF tmpVec;
        FxVectorF power;
        void init_buffers(int len)
        {
            tmpVecOrig = new FxVectorF( len );
            tmpVec = new FxVectorF( len );
            power = new FxVectorF( 256 );
        }

        void callback(ref byte[] data )
        {
            
            WaveBuffer wb = new WaveBuffer( data );
            
            if ( firstTime ) {

                // allocate all the buffers that we use
                init_buffers( data.Length/4);

                // set the zoom of the plot
                plot_signal_spectrum.Scale = new FxVector2f( 4117f );
                plot_signal_spectrum_original.Scale = new FxVector2f( 4117f );
                
                firstTime = false;
            }

            // set the external values
            tmpVecOrig.SetValue(wb.FloatBuffer);

            TimeStatistics.StartClock();

            if ( useFilter && this.fil != null ) {
                // use the filter directly
                this.fil.Transform( tmpVecOrig, tmpVec );

                // volumeup
                tmpVec.Multiply( vol );

                // pass back the data
                for ( int i = 0; i < tmpVec.Size; i++ )
                    wb.FloatBuffer[i] = tmpVec[i];
            } else {

                tmpVec.SetValue( tmpVecOrig );

                // volumeup
                tmpVec.Multiply( vol );

                // pass back the data
                for ( int i = 0; i < tmpVec.Size; i++ )
                    wb.FloatBuffer[i] = tmpVec[i];

            }

            FxVectorF tmpImag,tmpReal;

            // calc spectrum
            int mod = (int)Math.Ceiling( tmpVec.Size / ( (double)power.Size * 2 ) );

            // ===============================================================================================

            if ( ShowOutputAudioSpectrum ) {
                power.SetValue( 0.0f );
                FxVectorF local=tmpVec;
                if ( Math.Pow( 2, Math.Floor( Math.Log( local.Size, 2 ) ) ) != local.Size ) {
                    int newSize;

                    // calc the size of log2 
                    int sizeLog2 = (int)Math.Floor( Math.Log( local.Size, 2 ) )-1;

                    // calc the new size
                    newSize = (int)Math.Pow( 2, sizeLog2 );
                    if ( newSize < 512 )
                        newSize = 512;

                    local = tmpVec.GetSubVector( 0, newSize ) as FxVectorF;
                }

                mod = (int)Math.Ceiling( local.Size / ( (double)power.Size * 2 ) );

                // calc the fft 
                SignalTools.FFT_F( local, null, true, out tmpReal, out tmpImag );


                // pass all the data in form of blocks
                for ( int i = 0; i < mod; i++ ) {
                    for ( int j = 0; j < power.Size; j++ ) {
                        power[j] += (float)( Math.Sqrt( tmpReal[j * mod + i] * tmpReal[j * mod + i] + tmpImag[j * mod + i] * tmpImag[j * mod + i] ) );
                    }
                }

                // normalize the result
                power.Divide( mod * power.Size );

                // refresh the plot
                plot_signal_spectrum.RefreshPlot( power, 0 );

                // redraw canvas if we are not going to show output spectrum
                if ( !ShowInputAudioSpectrum )
                    canvas_audio.ReDraw();
            }

            // ===============================================================================================

            if ( ShowInputAudioSpectrum ) {
                // calc spectrum

                // reset the power
                power.SetValue(0.0f);
                FxVectorF local=tmpVecOrig;

                if ( Math.Pow( 2, Math.Floor( Math.Log( local.Size, 2 ) ) ) != local.Size ) {
                    int newSize;

                    // calc the size of log2 
                    int sizeLog2 = (int)Math.Floor( Math.Log( local.Size, 2 ) ) - 1;

                    // calc the new size
                    newSize = (int)Math.Pow( 2, sizeLog2 );
                    if ( newSize < 512 )
                        newSize = 512;

                    local = tmpVecOrig.GetSubVector( 0, newSize ) as FxVectorF;
                }

                mod = (int)Math.Ceiling( local.Size / ( (double)power.Size * 2 ) );

                // calc the fft 
                SignalTools.FFT_Safe_F( local, null, true, out tmpReal, out tmpImag );


                // pass all the data in form of blocks
                for ( int i = 0; i < mod; i++ ) {
                    for ( int j = 0; j < power.Size; j++ ) {
                        power[j] += (float)( Math.Sqrt( tmpReal[j * mod + i] * tmpReal[j * mod + i] + tmpImag[j * mod + i] * tmpImag[j * mod + i] ) );
                    }

                }

                // normalize the result
                power.Divide( mod * power.Size );

                // refresh the plot
                plot_signal_spectrum_original.RefreshPlot( power, 0 );

                // redraw canvas 
                canvas_audio.ReDraw();
            }

            TimeStatistics.StopClock( 1 );

        }


        PloterElement filterPlot;
        IFilter fil=null;
        void UpdateFilter(IFilter fil)
        {
            
            // calc the Freq response
            FxVectorF power = fil.GetFrequencyResponse(256);

            // scale down 
            power.Divide(power.Max());

            // scale up
            power.Multiply(125.0f / 4117.0f);

            // add the power of the filter to the plot
            plot_signal_spectrum.RefreshPlot(power, 1);

            // add the power of the filter to seperate plot with auto fitting of power
            plot_filter_spectrum.RefreshPlot(power, 0);
            plot_filter_spectrum.FitPlots();

            // refresh the canvas
            canvas_audio.ReDraw();

            // set the new filter for calculations
            this.fil = fil; 
        }

        private void button9_Click( object sender, EventArgs e )
        {
            // check if we have select file
            if ( String.IsNullOrEmpty( audioFileName ) ) {
                button8_Click( sender, e );
            }

            if ( String.IsNullOrEmpty( audioFileName ) ) {
                return;
            }

            // create wave out device
            CreateWaveOut();

            // create steram file
            ISampleProvider sampleProvider = null;

            try {

                // read the file
                fileWaveStream = new Mp3FileReader( audioFileName );

                // cretae the wave channel
                var waveChannel =  new SampleChannel( this.fileWaveStream );
                waveChannel.Volume = 0.5f;

                // create the sampler
                sampleProvider = new MeteringSampleProvider( waveChannel );

            } catch ( Exception createException ) {
                MessageBox.Show( String.Format( "{0}", createException.Message ), "Error Loading File" );
                return;
            }

            try {

                // in the wave out ( give and the call back for the editing
                waveOut.Init( new SampleToWaveProvider( sampleProvider ), new BufferCB( callback ) );

            } catch ( Exception initException ) {
                MessageBox.Show( String.Format( "{0}", initException.Message ), "Error Initializing Output" );
                return;
            }

            if ( plot_signal_spectrum == null )
            {

                #region Plot Creation

                signal_spectrum = new FxVectorF(256);

                // insert the plot of the time filter
                filterPlot = new PloterElement(signal_spectrum);
                filterPlot.Position.X = 0;
                filterPlot.Position.Y = 410;
                filterPlot.Origin.X = 10;
                filterPlot.Origin.Y = 100;
                filterPlot.FitPlots();
                canvas_audio.AddElements(filterPlot);

                // create the plot for the spectrum
                plot_signal_spectrum = new PloterElement( signal_spectrum );
                plot_signal_spectrum.Position.X = 0;
                plot_signal_spectrum.Position.Y = 10;
                plot_signal_spectrum.Origin.X = 10;
                plot_signal_spectrum.Origin.Y = 100;
                plot_signal_spectrum.FitPlots();
                plot_signal_spectrum.AddPlot(signal_spectrum, PlotType.Lines, Color.Aqua);

                // add the signal to canva
                canvas_audio.AddElements(plot_signal_spectrum);

                // create the plot for the spectrum
                plot_signal_spectrum_original = new PloterElement(signal_spectrum);
                plot_signal_spectrum_original.Position.X = 600;
                plot_signal_spectrum_original.Position.Y = 10;
                plot_signal_spectrum_original.Origin.X = 10;
                plot_signal_spectrum_original.Origin.Y = 100;
                plot_signal_spectrum_original.FitPlots();

                // add the signal to canva
                canvas_audio.AddElements(plot_signal_spectrum_original);


                // create the plot for the spectrum
                plot_filter_spectrum = new PloterElement(signal_spectrum);
                plot_filter_spectrum.Position.X = 600;
                plot_filter_spectrum.Position.Y = 410;
                plot_filter_spectrum.Origin.X = 10;
                plot_filter_spectrum.Origin.Y = 100;
                plot_filter_spectrum.FitPlots();

                // add the signal to canva
                canvas_audio.AddElements(plot_filter_spectrum);
                #endregion


                // add filter 
                UpdateFilter(BiQuadFilter.BandPassFilterConstantPeakGain(44100, 20000, 0.5f));


            }

            // start play
            waveOut.Play();
        }

        private void button10_Click( object sender, EventArgs e )
        {
            waveOut.Stop();
            CloseWaveOut();
        }

        #region Audio functions

        private void CreateWaveOut()
        {
            CloseWaveOut();
            int latency = 150;
            
#if USE_WAVE_OUT
            WaveOut outputDevice = new WaveOut( WaveCallbackInfo.NewWindow() );
            outputDevice.DeviceNumber = 0;
            outputDevice.DesiredLatency = latency;
            this.waveOut = outputDevice;
#else
            List<DirectSoundDeviceInfo> directDevice = DirectSoundOut.Devices.ToList < DirectSoundDeviceInfo>();
            this.waveOut = new DirectSoundOut( directDevice[0].Guid, latency );
#endif
        }

        private void CloseWaveOut()
        {
            if ( waveOut != null ) {
                waveOut.Stop();
            }
            if ( fileWaveStream != null ) {
                // this one really closes the file and ACM conversion
                fileWaveStream.Dispose();
            }
            if ( waveOut != null ) {
                waveOut.Dispose();
                waveOut = null;
            }
        }

        #endregion

        private void checkBox1_CheckedChanged( object sender, EventArgs e )
        {
            useFilter = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged( object sender, EventArgs e )
        {
            ShowInputAudioSpectrum = checkBox2.Checked;
        }

        private void checkBox3_CheckedChanged( object sender, EventArgs e )
        {
            ShowOutputAudioSpectrum = checkBox3.Checked;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (waveOut != null)
                waveOut.Stop();

            CloseWaveOut();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            FilterSelection fs = new FilterSelection();
            fs.cb = new UpdateFilterCb(UpdateFilter);
            fs.Show(this);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            vol = (trackBar1.Value / (float)trackBar1.Maximum)*5;
        }
        #endregion


        #region White Noise test

        // global variable for white noise
        FxVectorF wn = null;
        FxVectorF tmpWn = null;

        private void button12_Click( object sender, EventArgs e )
        {
            openFileDialog1.Filter = "White noise data file|*.dat";
            openFileDialog1.FilterIndex = 1;

            if ( openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

                TimeStatistics.StartClock();

                // read the file to the memmory and then parse the file

                List<string> lines = new List<string>();
                lines.AddRange( File.ReadAllLines( openFileDialog1.FileName ) );
                Console.WriteLine( lines.Count );

                //  create the vector for white noise data
                wn = new FxVectorF( lines.Count );

                int wnIndex =0;

                // parse the lines
                foreach ( string str in lines ) {
                    
                    // parse the float
                    wn[wnIndex] = float.Parse( str.Replace('.',','));

                    // increase the index for the wn vector
                    wnIndex++;
                }

                TimeStatistics.ClockLap( "File Load Parse Complete" );
                TimeStatistics.StopClock( 1 );
            }
        }

        private void button13_Click( object sender, EventArgs e )
        {
            // process the white noise data with the dsp
            if ( wn != null ) {

                tmpWn = new FxVectorF( wn.Size );


                // create the signal spectrum graphs
                if ( plot_signal_spectrum == null ) {

                    power = new FxVectorF( 256 );

                    #region Plot Creation

                    signal_spectrum = new FxVectorF( 256 );

                    // insert the plot of the time filter
                    filterPlot = new PloterElement( signal_spectrum );
                    filterPlot.Position.X = 0;
                    filterPlot.Position.Y = 410;
                    filterPlot.Origin.X = 10;
                    filterPlot.Origin.Y = 100;
                    filterPlot.FitPlots();
                    canvas_audio.AddElements( filterPlot );

                    // create the plot for the spectrum
                    plot_signal_spectrum = new PloterElement( signal_spectrum );
                    plot_signal_spectrum.Position.X = 0;
                    plot_signal_spectrum.Position.Y = 10;
                    plot_signal_spectrum.Origin.X = 10;
                    plot_signal_spectrum.Origin.Y = 100;
                    plot_signal_spectrum.FitPlots();
                    plot_signal_spectrum.AddPlot( signal_spectrum, PlotType.Lines, Color.Aqua );

                    // add the signal to canva
                    canvas_audio.AddElements( plot_signal_spectrum );

                    // create the plot for the spectrum
                    plot_signal_spectrum_original = new PloterElement( signal_spectrum );
                    plot_signal_spectrum_original.Position.X = 600;
                    plot_signal_spectrum_original.Position.Y = 10;
                    plot_signal_spectrum_original.Origin.X = 10;
                    plot_signal_spectrum_original.Origin.Y = 100;
                    plot_signal_spectrum_original.FitPlots();

                    // add the signal to canva
                    canvas_audio.AddElements( plot_signal_spectrum_original );


                    // create the plot for the spectrum
                    plot_filter_spectrum = new PloterElement( signal_spectrum );
                    plot_filter_spectrum.Position.X = 600;
                    plot_filter_spectrum.Position.Y = 410;
                    plot_filter_spectrum.Origin.X = 10;
                    plot_filter_spectrum.Origin.Y = 100;
                    plot_filter_spectrum.FitPlots();

                    // add the signal to canva
                    canvas_audio.AddElements( plot_filter_spectrum );
                    #endregion


                    // add filter 
                    UpdateFilter( BiQuadFilter.BandPassFilterConstantPeakGain( 44100, 20000, 0.5f ) );

                }

                if ( this.fil != null ) {
                    // use the filter directly
                    this.fil.Transform( wn, tmpWn );
                }


                FxVectorF tmpImag,tmpReal;

                // calc spectrum
                int mod = (int)Math.Ceiling( tmpWn.Size / ( (double)power.Size * 2 ) );

                // ===============================================================================================

                if ( ShowOutputAudioSpectrum ) {
                    power.SetValue( 0.0f );
                    FxVectorF local=tmpWn;
                    if ( Math.Pow( 2, Math.Floor( Math.Log( local.Size, 2 ) ) ) != local.Size ) {
                        int newSize;

                        // calc the size of log2 
                        int sizeLog2 = (int)Math.Floor( Math.Log( local.Size, 2 ) ) - 1;

                        // calc the new size
                        newSize = (int)Math.Pow( 2, sizeLog2 );
                        if ( newSize < 512 )
                            newSize = 512;

                        local = tmpWn.GetSubVector( 0, newSize ) as FxVectorF;
                    }

                    mod = (int)Math.Ceiling( local.Size / ( (double)power.Size * 2 ) );

                    // calc the fft 
                    SignalTools.FFT_F( local, null, true, out tmpReal, out tmpImag );


                    // pass all the data in form of blocks
                    for ( int i = 0; i < mod; i++ ) {
                        for ( int j = 0; j < power.Size; j++ ) {
                            power[j] += (float)( Math.Sqrt( tmpReal[j * mod + i] * tmpReal[j * mod + i] + tmpImag[j * mod + i] * tmpImag[j * mod + i] ) );
                        }
                    }

                    // normalize the result
                    power.Divide( power.Max() );

                    // refresh the plot
                    plot_signal_spectrum.RefreshPlot( power, 0 );
                    plot_signal_spectrum.FitPlots();

                    // redraw canvas if we are not going to show output spectrum
                    if ( !ShowInputAudioSpectrum )
                        canvas_audio.ReDraw();
                }

                // ===============================================================================================

                if ( ShowInputAudioSpectrum ) {
                    // calc spectrum

                    // reset the power
                    power.SetValue( 0.0f );
                    FxVectorF local=wn;

                    if ( Math.Pow( 2, Math.Floor( Math.Log( local.Size, 2 ) ) ) != local.Size ) {
                        int newSize;

                        // calc the size of log2 
                        int sizeLog2 = (int)Math.Floor( Math.Log( local.Size, 2 ) ) - 1;

                        // calc the new size
                        newSize = (int)Math.Pow( 2, sizeLog2 );
                        if ( newSize < 512 )
                            newSize = 512;

                        local = wn.GetSubVector( 0, newSize ) as FxVectorF;
                    }

                    mod = (int)Math.Ceiling( local.Size / ( (double)power.Size * 2 ) );

                    // calc the fft 
                    SignalTools.FFT_Safe_F( local, null, true, out tmpReal, out tmpImag );


                    // pass all the data in form of blocks
                    for ( int i = 0; i < mod; i++ ) {
                        for ( int j = 0; j < power.Size; j++ ) {
                            power[j] += (float)( Math.Sqrt( tmpReal[j * mod + i] * tmpReal[j * mod + i] + tmpImag[j * mod + i] * tmpImag[j * mod + i] ) );
                        }

                    }

                    // normalize the result
                    power.Divide( power.Max() );

                    // refresh the plot
                    plot_signal_spectrum_original.RefreshPlot( power, 0 );
                    plot_signal_spectrum_original.FitPlots();


                    // redraw canvas 
                    canvas_audio.ReDraw();
                }
            }
        }

        private void button14_Click( object sender, EventArgs e )
        {
            // save the vector to file
            saveFileDialog1.Filter = "White noise data file|*.dat";
            saveFileDialog1.FilterIndex = 1;

            if ( saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {

                // create the text
                StringBuilder sb = new StringBuilder();
                foreach (float fl in tmpWn.Data){
                    sb.AppendLine( fl.ToString() );
                }

                sb = sb.Replace( ',', '.' );

                // create a writer and open the file
                TextWriter tw = new StreamWriter( saveFileDialog1.FileName );

                // write a line of text to the file
                tw.Write( sb.ToString() );

                // close the stream
                tw.Close();

            }

        }

        #endregion

    }

}
