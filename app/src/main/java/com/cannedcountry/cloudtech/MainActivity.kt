package com.cannedcountry.cloudtech

import android.app.PendingIntent
import android.appwidget.AppWidgetManager
import android.content.ComponentName
import android.content.Context
import android.content.Intent
import android.os.Build
import android.os.Bundle
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.cannedcountry.cloudtech.databinding.ActivityMainBinding

class MainActivity : AppCompatActivity() {

    private lateinit var binding: ActivityMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setupClickListeners()
    }

    private fun setupClickListeners() {
        val homeUrl = getString(R.string.url_home)
        val awsUrl = getString(R.string.url_aws_course)
        val certVideoUrl = getString(R.string.url_cert_video)
        val newCertUrl = getString(R.string.url_new_cert)

        // 1. ホーム
        binding.cardHome.setOnClickListener {
            UrlOpener.openUrl(this, homeUrl)
        }

        // 2. AWSコース一覧
        binding.cardAwsCourse.setOnClickListener {
            UrlOpener.openUrl(this, awsUrl)
        }

        // 3. 資格対策動画
        binding.cardCertVideo.setOnClickListener {
            UrlOpener.openUrl(this, certVideoUrl)
        }

        // 4. 新資格対策
        binding.cardNewCert.setOnClickListener {
            UrlOpener.openUrl(this, newCertUrl)
        }

        // ウィジェットをホーム画面に追加ボタン
        binding.btnAddWidgetToHome.setOnClickListener {
            requestPinWidget()
        }
    }

    private fun requestPinWidget() {
        val appWidgetManager = getSystemService(Context.APPWIDGET_SERVICE) as? AppWidgetManager
        val myProvider = ComponentName(this, CloudTechWidgetProvider::class.java)

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            if (appWidgetManager != null && appWidgetManager.isRequestPinAppWidgetSupported) {
                appWidgetManager.requestPinAppWidget(myProvider, null, null)
                Toast.makeText(this, R.string.toast_widget_requested, Toast.LENGTH_SHORT).show()
            } else {
                Toast.makeText(this, R.string.toast_widget_not_supported, Toast.LENGTH_LONG).show()
            }
        } else {
            Toast.makeText(this, R.string.toast_widget_not_supported, Toast.LENGTH_LONG).show()
        }
    }
}
