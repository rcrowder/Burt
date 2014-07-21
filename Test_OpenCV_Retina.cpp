//#include <opencv2/core/core.hpp>
//#include <opencv2/highgui/highgui.hpp>
//#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/opencv.hpp>
#include <iostream>
#include <vector>

//using namespace cv;
using namespace std;

// Function is used to compute the q values in the equation
float Px(int init, int end, cv::Mat& hist)
{
    float sum = 0.f;
    for (int i = init; i <= end; i++)
        sum += hist.at<float>(i);

    return (float)sum;
}

// Function is used to compute the mean values in the equation (mu)
float Mx(int init, int end, cv::Mat& hist)
{
    float sum = 0.f;
    for (int i = init; i <= end; i++)
        sum += i * hist.at<float>(i);

    return (float)sum;
}

// A Threshold Selection Method from Gray-Level Histograms - NOBUYUKI OTSUA
//
// A nonparametric and unsupervised method of automatic 
// threshold selection for picture segmentation is presented.
// An optimal threshold is selected by the discriminant 
// criterion, namely, so as to maximize the separability 
// of the resultant classes in gray levels.
//
//http://ieeexplore.ieee.org/stamp/stamp.jsp?tp=&arnumber=4310076
//
cv::MatND hist;
int histSize = 256;
float ranges[] = { 0, 256 };

int getOtsuThreshold(cv::Mat& src, float *maxValue)
{
	int t = 0;
		
    cv::Mat p(src);
	if (p.channels() == 3)
	    cvtColor(p, p, CV_BGR2GRAY);

	calcHist(&p, 1, 0, cv::Mat(), hist, 1, &histSize, 0);

	// Loop through all possible t values and maximize between class variance
	vector<float> vec (256);
	float p1,p2,p12;
    float maxVec = 0;
    float maxHist = 0;

	for (int k = 1; k != 255; k++)
	{
		p1 = Px(0, k, hist);
		p2 = Px(k + 1, 255, hist);

		p12 = p1 * p2;
		if (p12 == 0) 
			p12 = 1;

		float diff = (Mx(0, k, hist) * p2) - (Mx(k + 1, 255, hist) * p1);
		vec[k] = (float)diff * diff / p12;
	//	vec[k] = (float)powf((Mx(0, k, hist) * p2) - (Mx(k + 1, 255, hist) * p1), 2.f) / p12;

		if (vec[k] > maxVec)
        {
            maxVec = vec[k];
            t = k; // grab the index
        }
		if (hist.at<float>(k) > maxHist)
		{
			maxHist = hist.at<float>(k);
		}
	}
	if (maxValue)
		*maxValue = maxHist;

    cv::Mat histImage = cv::Mat::ones(256, 256, CV_8U)*255;
    histImage = cv::Scalar::all(255);

	normalize(hist, hist, 0, histImage.rows, CV_MINMAX, CV_32F);

    int binW = cvRound((double)histImage.cols/histSize);
    for (int s = 0; s < histSize; s++ )
        rectangle( histImage, 
					cv::Point(s*binW,	  histImage.rows),
					cv::Point((s+1)*binW, histImage.rows - cvRound(hist.at<float>(s))),
					cv::Scalar::all(0), 
					-1, 8, 0 );

	// Plot on the histogram image a vertical rectangle at t, t+1
	rectangle( histImage,
				cv::Point(t*binW,	  histImage.rows),
				cv::Point((t+1)*binW, histImage.rows - cvRound(hist.at<float>(t))),
				cv::Scalar::all(255),
				-1, 8, 0 );
	rectangle( histImage,
				cv::Point(t*binW,	  histImage.rows - cvRound(hist.at<float>(t))),
				cv::Point((t+1)*binW, 0),
				cv::Scalar::all(0),
				-1, 8, 0 );

	cv::imshow("Magno Histogram", histImage);

	return t;
}


int main( int argc, char** argv )
{
	// Declare the retina input buffer... that will be fed differently in regard of the input media
	cv::Mat inputFrame;
	cv::VideoCapture videoCapture; // in case a video media is used, its manager is declared here

	videoCapture.open(0);

	// grab a first frame to check if everything is ok
	videoCapture >> inputFrame;

	if (inputFrame.empty())
		inputFrame = cv::imread("./Burt.jpg");

	resize(inputFrame, inputFrame, cv::Size(), 0.5, 0.5, CV_INTER_AREA);

	//////////////////////////////////////////////////////////////////////////////
	// Program start in a try/catch safety context (Retina may throw errors)
	try
	{
		// create a retina instance with default parameters setup, uncomment the initialisation you wanna test
		cv::Ptr<cv::Retina> myRetina;

		// Activate log sampling (favour foveal vision and subsamples peripheral vision)
		//myRetina = new cv::Retina(inputFrame.size(), true, cv::RETINA_COLOR_BAYER, true, 2.0, 10.0);
		// Allocate "classical" retina :
		myRetina = new cv::Retina(inputFrame.size());

		// save default retina parameters file in order to let you see this and maybe modify it and reload using method "setup"
		//myRetina->write("RetinaDefaultParameters.xml");

		// load parameters if file exists
		myRetina->setup("RetinaSpecificParameters.xml");
		myRetina->clearBuffers();

		// declare retina output buffers
		cv::Mat retinaOutput_parvo, prev_retinaOutput_parvo;
		cv::Mat retinaOutput_magno, prev_retinaOutput_magno;

		cv::Mat retinaThreshold_magno;

		cv::Mat retinaAreaOfInterest;

		cv::Mat retinaFlow_magno;
		cv::Mat retinaFlow_parvo;

		myRetina->run(inputFrame);

		// processing loop with stop condition
		while(true)
		{
			// If using video stream, then, grabbing a new frame, else, input remains the same
			
			if (videoCapture.isOpened())
			{
				videoCapture>>inputFrame;
				resize(inputFrame, inputFrame, cv::Size(), 0.5, 0.5, CV_INTER_AREA);
			}

			// Run retina filter
			
			myRetina->run(inputFrame);

   			// Retrieve and display retina output
			
			prev_retinaOutput_parvo = retinaOutput_parvo;
			// -> foveal color vision details channel with luminance and noise correction
			myRetina->getParvo(retinaOutput_parvo);

			prev_retinaOutput_magno = retinaOutput_magno;
			// -> peripheral monochrome motion and events (transient information) channel
			myRetina->getMagno(retinaOutput_magno);

			float maxMagnoHistVal = 0.f;
			int Magno_OtsuThreshold = getOtsuThreshold(retinaOutput_magno, &maxMagnoHistVal);
			double high_thres = cv::threshold(retinaOutput_magno, retinaThreshold_magno, (double)Magno_OtsuThreshold, 255.0, CV_THRESH_TOZERO);

			retinaAreaOfInterest = cv::Scalar(0);
			retinaOutput_parvo.copyTo(retinaAreaOfInterest, retinaThreshold_magno);

			vector<vector<cv::Point>> contours;
			vector<cv::Vec4i> hierarchy;
			Canny(retinaAreaOfInterest, retinaAreaOfInterest, 0.5*high_thres, (double)Magno_OtsuThreshold);//high_thres); // 0.66*mean,1.33*mean
			cv::Mat grey = cv::Mat::zeros(retinaAreaOfInterest.rows, retinaAreaOfInterest.cols, CV_8UC1);
			retinaAreaOfInterest.convertTo(grey, CV_8UC1);
			cv::findContours(grey, contours, hierarchy, CV_RETR_CCOMP, CV_CHAIN_APPROX_SIMPLE);

			if (!contours.empty())
			{
				// iterate through all the top-level contours,
				// draw each connected component with its own random color
				int idx = 0;
				for( ; idx >= 0; idx = hierarchy[idx][0] )
				{
					cv::Scalar color(rand()&255, rand()&255, rand()&255);
					drawContours(retinaOutput_parvo, contours, idx, color, CV_FILLED, 8, hierarchy);
				}
			}

			cv::imshow("Retina Input",			inputFrame);
			cv::imshow("Retina Parvo",			retinaOutput_parvo);
			cv::imshow("Retina Magno",			retinaOutput_magno);
			cv::imshow("Magno Otsu Threshold",	retinaThreshold_magno);
			cv::imshow("Area Of Interest",		retinaAreaOfInterest);

			cv::moveWindow("Retina Input",			0,						  0);
			cv::moveWindow("Retina Parvo",			((inputFrame.cols+16)*1), 0);
			cv::moveWindow("Retina Magno",			((inputFrame.cols+16)*2), 0);
			cv::moveWindow("Magno Histogram",		((inputFrame.cols+16)*0), ((inputFrame.rows+32)*1));
			cv::moveWindow("Area Of Interest",		((inputFrame.cols+16)*1), ((inputFrame.rows+32)*1));
			cv::moveWindow("Magno Otsu Threshold",	((inputFrame.cols+16)*2), ((inputFrame.rows+32)*1));

			if (cv::waitKey(30) >= 0)
				break;
		}
	}
	catch(cv::Exception e)
	{
		const char* err_msg = e.what();
		std::cerr<<"Error using Retina : " << err_msg << std::endl;
		std::cout<<"Error using Retina : " << err_msg << std::endl;
	}

	// Program end message
	std::cout << "Retina demo end" << std::endl;

	return 0;
}
