package com.cannedcountry.cloudtech

import android.app.PendingIntent
import android.appwidget.AppWidgetManager
import android.appwidget.AppWidgetProvider
import android.content.ComponentName
import android.content.Context
import android.content.Intent
import android.net.Uri
import android.widget.RemoteViews

class CloudTechWidgetProvider : AppWidgetProvider() {

    override fun onUpdate(
        context: Context,
        appWidgetManager: AppWidgetManager,
        appWidgetIds: IntArray
    ) {
        for (appWidgetId in appWidgetIds) {
            updateAppWidget(context, appWidgetManager, appWidgetId)
        }
    }

    companion object {
        fun updateAppWidget(
            context: Context,
            appWidgetManager: AppWidgetManager,
            appWidgetId: Int
        ) {
            val views = RemoteViews(context.packageName, R.layout.widget_cloudtech)

            val homeUrl = context.getString(R.string.url_home)
            val awsUrl = context.getString(R.string.url_aws_course)
            val certVideoUrl = context.getString(R.string.url_cert_video)
            val newCertUrl = context.getString(R.string.url_new_cert)

            views.setOnClickPendingIntent(
                R.id.btn_widget_home,
                createOpenUrlPendingIntent(context, homeUrl, 101)
            )
            views.setOnClickPendingIntent(
                R.id.btn_widget_aws,
                createOpenUrlPendingIntent(context, awsUrl, 102)
            )
            views.setOnClickPendingIntent(
                R.id.btn_widget_cert_video,
                createOpenUrlPendingIntent(context, certVideoUrl, 103)
            )
            views.setOnClickPendingIntent(
                R.id.btn_widget_new_cert,
                createOpenUrlPendingIntent(context, newCertUrl, 104)
            )

            appWidgetManager.updateAppWidget(appWidgetId, views)
        }

        private fun createOpenUrlPendingIntent(
            context: Context,
            url: String,
            requestCode: Int
        ): PendingIntent {
            val intent = Intent(context, OpenUrlActivity::class.java).apply {
                data = Uri.parse(url)
                flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TOP
            }
            return PendingIntent.getActivity(
                context,
                requestCode,
                intent,
                PendingIntent.FLAG_UPDATE_CURRENT or PendingIntent.FLAG_IMMUTABLE
            )
        }
    }
}
