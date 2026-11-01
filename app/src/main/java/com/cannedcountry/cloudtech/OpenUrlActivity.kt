package com.cannedcountry.cloudtech

import android.app.Activity
import android.os.Bundle

class OpenUrlActivity : Activity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val targetUrl = intent?.dataString ?: intent?.getStringExtra(EXTRA_URL)
        if (!targetUrl.isNullOrEmpty()) {
            UrlOpener.openUrl(this, targetUrl)
        }

        finish()
    }

    companion object {
        const val EXTRA_URL = "com.cannedcountry.cloudtech.EXTRA_URL"
    }
}
